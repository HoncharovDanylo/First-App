using Api.Context;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CardController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
            
        [HttpGet("/cards")]       
        public async Task<IActionResult> GetCards()
        {
            var cards = await _dbContext.Cards.Include(x=>x.TaskList).Where(card=>!card.IsDeleted).Select(x=>new CardDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                TaskListName = x.TaskList.Name,
                TaskListId = x.TaskList.Id,
                Priority = x.Priority
            }).ToListAsync();
            return Ok(cards);
        }
        [HttpGet("/cards/{id}")]       
        public async Task<IActionResult> GetCard(int? id)
        {
            if (id == null)
                return NotFound();
            var card = await _dbContext.Cards.Include(c=>c.TaskList).Where(card=>!card.IsDeleted).SingleOrDefaultAsync(card=>card.Id == id);
            if (card == null)
                return NotFound();
            var cardDto = new CardDto()
            {
                Id = card.Id,
                Title = card.Title,
                Description = card.Description,
                DueDate = card.DueDate,
                TaskListName = card.TaskList.Name,
                TaskListId = card.TaskList.Id,
                Priority = card.Priority
            };
            return Ok(cardDto);
        }
        [HttpPost("/cards/create")]
        public async Task<IActionResult> CreateCard([FromBody] CreateUpdateCardDto cardDto)
        {
            if (ModelState.IsValid)
            {
                var card = new Card()
                {
                    Title = cardDto.Title.Trim(),
                    Description = cardDto.Description.Trim(),
                    DueDate = cardDto.DueDate,
                    TaskListId = cardDto.TaskListId,
                    Priority = cardDto.Priority.Trim()
                };
                await _dbContext.Cards.AddAsync(card);
                await _dbContext.SaveChangesAsync();
                
                return Ok();
                
            }

            return BadRequest("valid error");
        }

        [HttpPatch("/cards/update/{id}")]
        public async Task<IActionResult> UpdateCard(int id, CreateUpdateCardDto updateDto)
        {
            if (id == null)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var card = await _dbContext.Cards.Where(card=>!card.IsDeleted).SingleOrDefaultAsync(card => card.Id == id); 
                if (card == null)
                    return NotFound();
                card.Title = updateDto.Title.Trim();
                card.Description = updateDto.Description.Trim();
                card.DueDate = updateDto.DueDate;
                card.TaskListId = updateDto.TaskListId;
                card.Priority = updateDto.Priority.Trim();
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("/cards/delete/{id}")]
        public async Task<IActionResult> DeleteCard(int? id)
        {
            if (id == null)
                return NotFound();
            var card = await _dbContext.Cards.Where(card=>!card.IsDeleted).SingleOrDefaultAsync(card=>card.Id == id);
            if (card == null)
                return NotFound();
            _dbContext.Cards.Remove(card);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("/card/{cardId}/change-list/{taskListId}")]
        public async Task<IActionResult> ChangeList(int cardId, int taskListId)
        {
            if (cardId == null)
                return NotFound();
            var card = await _dbContext.Cards.Where(card=>!card.IsDeleted).SingleOrDefaultAsync(card=>card.Id == cardId);
            if (card == null)
                return NotFound();
            card.TaskListId = taskListId;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
