using Api.Context;
using Api.Models.DTOs.HistoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        
        public HistoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/history/{page}")]
        public async Task<IActionResult> GetHistory(int page)
        {
            var history = await _dbContext.Histories.OrderByDescending(x=>x.Timestamp).Include(x=>x.Card).Skip((page-1)*20).Take(20).Select(x=>new HistoryDto()
            {
                Action = x.Action,
                CardName = x.Card.Title,
                Field = x.Field,
                NewValue = x.NewValue,
                PreviousValue = x.PreviousValue,
                Timestamp = x.Timestamp
            }).ToListAsync();
            return Ok(history);
        }
        [HttpGet("/history/card/{cardId}")]
        public async Task<IActionResult> GetHistoryById(int? cardId)
        {
            if (cardId == null)
                return NotFound();
            var history = await _dbContext.Histories.Include(x=>x.Card).SingleOrDefaultAsync(h=>h.CardId == cardId);
            if (history == null)
                return NotFound();
            return Ok(new HistoryDto()
            {
                Action = history.Action,
                CardName = history.Card.Title,
                Field = history.Field,
                NewValue = history.NewValue,
                PreviousValue = history.PreviousValue,
                Timestamp = history.Timestamp
            });
        }
    }
}
