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
        private readonly IValidator<CreateUpdateCardDto> _validator;
        private readonly ICardRepository _cardRepository;
        private readonly ITaskListRepository _taskListRepository;

        public CardController(IValidator<CreateUpdateCardDto> validator, ICardRepository cardRepository, ITaskListRepository taskListRepository)
        {
            _validator = validator;
            _cardRepository = cardRepository;
            _taskListRepository = taskListRepository;
        }
        
        [HttpGet("/cards/by-board/{boardId}")]
        public async Task<IActionResult> GetCardsByBoard(int boardId)
        {
            var cards = (await _cardRepository.GetByBoardId(boardId)).Select(x => new CardDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                TaskListName = x.TaskList.Name,
                TaskListId = x.TaskList.Id,
                Priority = x.Priority
            });
            return Ok(cards);
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
                    DueDate = DateOnly.FromDateTime(cardDto.DueDate),
                    TaskListId = cardDto.TaskListId,
                    Priority = cardDto.Priority.Trim()
                };
                await _cardRepository.Create(card);
                var responseCardDto = new CardDto()
                {
                    Id = card.Id,
                    Title = card.Title,
                    Description = card.Description,
                    DueDate = card.DueDate,
                    TaskListName = card.TaskList.Name,
                    TaskListId = card.TaskList.Id,
                    Priority = card.Priority
                };
                return Ok(responseCardDto);
                
            }

            return BadRequest("Validation error occured");
        }

        [HttpPut("/cards/update/{id}")]
        public async Task<IActionResult> UpdateCard(int id, [FromBody]CreateUpdateCardDto updateDto)
        {
            if ((await _validator.ValidateAsync(updateDto)).IsValid)
            {
                var card = await _cardRepository.GetById(id); 
                var newList = await _taskListRepository.GetById(updateDto.TaskListId);
                if (card == null || newList == null)
                    return NotFound();
                if (card.TaskList.BoardId != newList.BoardId)
                    return BadRequest();
               
                
                
                card.Title = updateDto.Title.Trim();
                card.Description = updateDto.Description.Trim();
                card.DueDate = DateOnly.FromDateTime(updateDto.DueDate);
                card.TaskListId = updateDto.TaskListId;
                card.Priority = updateDto.Priority.Trim();
                
                await _cardRepository.Update(card);
                return Ok();
            }
            return BadRequest("valid error");
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
            var newList = await _taskListRepository.GetById(taskListId);
            if(card == null || newList == null)
                return NotFound();
            if (card.TaskList.BoardId != newList.BoardId)
                return BadRequest();
            card.TaskListId = taskListId;
            await _cardRepository.Update(card);
            return Ok();
        }

    }
}
