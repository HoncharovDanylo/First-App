using Api.Context;
using Api.Interfaces;
using Api.Models.DTOs.HistoryDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepository _historyRepository;
        
        public HistoryController(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;
        }

        [HttpGet("/history/board/{boardId}/{page}")]
        public async Task<IActionResult> GetHistoryByBoard(int boardId, int page)
        {
            var history = (await _historyRepository.GetByBoard(boardId,page)).Select(x=>new HistoryDto()
            {
                Action = x.Action,
                CardName = x.CardTitle,
                ListName = x.ListName,
                Field = x.Field,
                NewValue = x.NewValue,
                PreviousValue = x.PreviousValue,
                Timestamp = x.Timestamp
            });
            return Ok(history);
        }
        [HttpGet("/history/card/{cardId}")]
        public async Task<IActionResult> GetHistoryById(int? cardId)
        {
            if (cardId == null)
                return NotFound();
            var history = (await _historyRepository.GetByCardId(cardId.Value)).Select(history=>new HistoryDto()
            {
                
                Action = history.Action,
                CardName = history.CardTitle,
                ListName = history.ListName,
                Field = history.Field,
                NewValue = history.NewValue,
                PreviousValue = history.PreviousValue,
                Timestamp = history.Timestamp
            });
            if (history == null)
                return NotFound();
            return Ok(history);
        }
    }
}
