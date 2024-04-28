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
            var history = await _dbContext.Histories.OrderByDescending(x=>x.Timestamp).Skip((page-1)*20).Take(20).Select(x=>new HistoryDto()
            {
                Action = x.Action,
                CardName = x.CardTitle,
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
            var history = await _dbContext.Histories.Where(h=>h.CardId == cardId).Select(history=>new HistoryDto()
            {
                Action = history.Action,
                CardName = history.CardTitle,
                Field = history.Field,
                NewValue = history.NewValue,
                PreviousValue = history.PreviousValue,
                Timestamp = history.Timestamp
            }).ToListAsync();
            if (history == null)
                return NotFound();
            return Ok();
        }
    }
}
