using Api.Context;
using Api.Extensions;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<CreateTaskListDto> _validator;
        private readonly ITaskListRepository _taskListRepository;

        public TaskListController(ApplicationDbContext dbContext, IValidator<CreateTaskListDto> validator, ITaskListRepository taskListRepository)
        {
            _dbContext = dbContext;
            _validator = validator;
            _taskListRepository = taskListRepository;
        }
        
        [HttpGet("/lists")]
        public async Task<IActionResult> GetTaskLists()
        {
            var taskLists = (await _taskListRepository.GetTaskLists()).Select(x => new TaskListDto()
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
                CardsCount = x.Cards.Count
            });
            return Ok(taskLists);
        }

        [HttpGet("list/movements/{id:int}")]
        public async Task<IActionResult> GetTaskListNames(int id)
        {
            var taskList = (await _taskListRepository.GetTaskLists()).Select(x=> new CardlessTaskListDto() { Id = x.Id, Name = x.Name }).Where(x=>x.Id != id);
            return Ok(taskList);
        }
        
        [HttpGet("list/movements/all/{id:int}")]
        public async Task<IActionResult> GetAllTaskListNames(int id)
        {
            var taskList = (await _taskListRepository.GetTaskLists()).Select(x=> new CardlessTaskListDto() { Id = x.Id, Name = x.Name }).ToList();
            taskList.MoveToFront(x => x.Id == id);
            return Ok(taskList);
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
                CardsCount = taskList.Cards.Count
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
                    Name = taskListDto.Name.Trim()
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
                var taskList = await _dbContext.TaskLists.SingleOrDefaultAsync(tl => tl.Id == id);
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
            var taskList = await _dbContext.TaskLists.SingleOrDefaultAsync(tl=>tl.Id == id);
            if (taskList == null)
                return NotFound();
            await _taskListRepository.Delete(taskList);
            return Ok();
        }
    }
}
