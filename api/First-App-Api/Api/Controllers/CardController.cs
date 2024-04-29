using Api.Context;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;
using Api.Validations;
using FluentValidation;
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
        private readonly IValidator<CreateUpdateCardDto> _validator;
        private readonly ICardRepository _cardRepository;

        public CardController(ApplicationDbContext dbContext, IValidator<CreateUpdateCardDto> validator, ICardRepository cardRepository)
        {
            _dbContext = dbContext;
            _validator = validator;
            _cardRepository = cardRepository;
        }
            
        
        [HttpGet("/cards/{id}")]       
        public async Task<IActionResult> GetCard(int? id)
        {
            if (id == null)
                return NotFound();
            var card = await _cardRepository.GetById(id.Value);
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
            if ((await _validator.ValidateAsync(cardDto)).IsValid)
            {
                var card = new Card()
                {
                    Title = cardDto.Title.Trim(),
                    Description = cardDto.Description.Trim(),
                    DueDate = cardDto.DueDate,
                    TaskListId = cardDto.TaskListId,
                    Priority = cardDto.Priority.Trim()
                };
                await _cardRepository.Create(card);
                
                return Ok();
                
            }

            return BadRequest("valid error");
        }

        [HttpPut("/cards/update/{id}")]
        public async Task<IActionResult> UpdateCard(int? id, CreateUpdateCardDto updateDto)
        {
            if (id == null)
                return BadRequest();
            if ((await _validator.ValidateAsync(updateDto)).IsValid)
            {
                var card = await _cardRepository.GetById(id.Value); 
                if (card == null)
                    return NotFound();
                
                card.Title = updateDto.Title.Trim();
                card.Description = updateDto.Description.Trim();
                card.DueDate = updateDto.DueDate;
                card.TaskListId = updateDto.TaskListId;
                card.Priority = updateDto.Priority.Trim();
                
                await _cardRepository.Update(card);
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("/cards/delete/{id}")]
        public async Task<IActionResult> DeleteCard(int? id)
        {
            if (id == null)
                return NotFound();
            var card = await _cardRepository.GetById(id.Value);
            if (card == null)
                return NotFound();
            await _cardRepository.Delete(card);
            return Ok();
        }
        [HttpPatch("/card/{cardId}/change-list/{taskListId}")]
        public async Task<IActionResult> ChangeList(int cardId, int taskListId)
        {
            var card = await _cardRepository.GetById(cardId);
            if (card == null)
                return NotFound();
            card.TaskListId = taskListId;
            await _cardRepository.Update(card);
            return Ok();
        }

    }
}
