using Api.Context;
using Api.Models;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    public class TaskListController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskListController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        [HttpGet("/lists")]
        public async Task<IActionResult> GetTaskLists()
        {
            var taskLists = await _dbContext.TaskLists.Select(x=>new TaskListDto()
            {
                Id = x.Id,
                Name = x.Name.Trim(),
                Cards = x.Cards.Select(c=>new CardDto()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    DueDate = c.DueDate,
                    Priority = c.Priority,
                    TaskListId = x.Id,
                    TaskListName = x.Name
                }).ToList(),
                CardsCount = x.Cards.Count
            }).ToListAsync();
            return Ok(taskLists);
        }

        [HttpGet("list/movements/{id:int?}")]
        public async Task<IActionResult> GetTaskListNames(int? id)
        {
            var taskList = _dbContext.TaskLists.Select(x=> new CardlessTaskListDto() { Id = x.Id, Name = x.Name });
            if(id!=null)
                taskList = taskList.Where(x=>x.Id != id);
            return Ok(taskList);
        }
        [HttpGet("/lists/{id}")]
        public async Task<IActionResult> GetTaskList(int? id)
        {
            if (id == null)
                return NotFound();
            var taskList = await _dbContext.TaskLists.Include(tl=>tl.Cards).SingleOrDefaultAsync(tl=>tl.Id == id);
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
                }).ToList(),
                CardsCount = taskList.Cards.Count
            };
            return Ok(taskListDto);
        }
        [HttpPost("/lists/create")]
        public async Task<IActionResult> CreateTaskList([FromBody] CreateTaskListDto taskListDto)
        {
            if (ModelState.IsValid)
            {
                var taskList = new TaskList()
                {
                    Name = taskListDto.Name.Trim()
                };
                await _dbContext.TaskLists.AddAsync(taskList);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("lists/{id}")]
        public async Task<IActionResult> UpdateTaskList(int? id, [FromBody] CreateTaskListDto taskListDto)
        {
            if (id == null)
                return NotFound();
            var taskList = await _dbContext.TaskLists.SingleOrDefaultAsync(tl=>tl.Id == id);
            if (taskList == null)
                return NotFound();
            taskList.Name = taskListDto.Name.Trim();
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("/lists/{id}")]
        public async Task<IActionResult> DeleteTaskList(int? id)
        {
            if (id == null)
                return NotFound();
            var taskList = await _dbContext.TaskLists.SingleOrDefaultAsync(tl=>tl.Id == id);
            if (taskList == null)
                return NotFound();
            _dbContext.TaskLists.Remove(taskList);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
