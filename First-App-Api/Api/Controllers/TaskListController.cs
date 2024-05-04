using Api.Context;
using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly IValidator<CreateTaskListDto> _validator;
        private readonly ITaskListRepository _taskListRepository;

        public TaskListController( IValidator<CreateTaskListDto> validator, ITaskListRepository taskListRepository)
        {
            _validator = validator;
            _taskListRepository = taskListRepository;
        }
        
        [HttpGet("/lists/by-board/{boardId}")]
        public async Task<IActionResult> GetTaskLists(int boardId)
        {
            var taskLists = (await _taskListRepository.GetTaskLists()).Where(x=>x.BoardId == boardId).Select(x => new TaskListDto()
            {
                Id = x.Id,
                Name = x.Name.Trim(),
                Cards = x.Cards.Select(c => new CardDto()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    DueDate = c.DueDate,
                    Priority = c.Priority,
                    TaskListId = x.Id,
                    TaskListName = x.Name
                }).OrderBy(x=>x.DueDate).ToList(),
                CardsCount = x.Cards.Count,
                BoardId = x.BoardId
            });
            return Ok(taskLists);
        }

        [HttpGet("list/movements/{id:int}")]
        public async Task<IActionResult> GetTaskListNames(int id)
        {
            var list = (await _taskListRepository.GetById(id));
            var taskLists = (await _taskListRepository.GetTaskLists())
                .Where(x=>x.Id != id && x.BoardId == list.BoardId)
                .Select(x=> new CardlessTaskListDto() { Id = x.Id, Name = x.Name });
            return Ok(taskLists);
        }
        
        [HttpGet("list/movements/all/{id:int}")]
        public async Task<IActionResult> GetAllTaskListNames(int id)
        {
            var list = (await _taskListRepository.GetById(id));
            var taskLists = (await _taskListRepository.GetTaskLists())
                .Where(x=>x.BoardId == list.BoardId)
                .Select(x=> new CardlessTaskListDto() { Id = x.Id, Name = x.Name }).ToList();
            taskLists.MoveToFront(x => x.Id == id);
            return Ok(taskLists);
        }
        [HttpGet("/lists/{id}")]
        public async Task<IActionResult> GetTaskList(int? id)
        {
            if (id == null)
                return NotFound();
            var taskList = await _taskListRepository.GetById(id.Value);
            if (taskList == null)
                return NotFound();
            var taskListDto = new TaskListDto()
            {
                Id = taskList.Id,
                Name = taskList.Name,
                Cards = taskList.Cards.Select(c => new CardDto()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    DueDate = c.DueDate,
                    Priority = c.Priority
                }).OrderBy(x=>x.DueDate).ToList(),
                CardsCount = taskList.Cards.Count,
                BoardId = taskList.BoardId
            };
            return Ok(taskListDto);
        }
        [HttpPost("/lists/create")]
        public async Task<IActionResult> CreateTaskList([FromBody] CreateTaskListDto taskListDto)
        {
            if ((await _validator.ValidateAsync(taskListDto)).IsValid)
            {
                var taskList = new TaskList()
                {
                    Name = taskListDto.Name.Trim(),
                    BoardId = taskListDto.BoardId
                };
                await _taskListRepository.Create(taskList);
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("lists/{id}")]
        public async Task<IActionResult> UpdateTaskList(int? id, [FromBody] CreateTaskListDto taskListDto)
        {
            if ((await _validator.ValidateAsync(taskListDto)).IsValid)
            {


                if (id == null)
                    return NotFound();
                var taskList = await _taskListRepository.GetById(id.Value);
                if (taskList == null)
                    return NotFound();
                taskList.Name = taskListDto.Name.Trim();
                await _taskListRepository.Update(taskList);
                return Ok();
            }

            return BadRequest();
        }
        [HttpDelete("/lists/{id}")]
        public async Task<IActionResult> DeleteTaskList(int? id)
        {
            if (id == null)
                return NotFound();
            var taskList = await _taskListRepository.GetById(id.Value);
            if (taskList == null)
                return NotFound();
            await _taskListRepository.Delete(taskList);
            return Ok();
        }
    }
}
