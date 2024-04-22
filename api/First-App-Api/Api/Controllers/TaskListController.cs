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
                Name = x.Name,
                Cards = x.Cards.Select(c=>new CardDto()
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    DueDate = c.DueDate,
                    Priority = c.Priority,
                }).ToList(),
                CardsCount = x.Cards.Count
            }).ToListAsync();
            return Ok(taskLists);
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
                    Name = taskListDto.Name
                };
                await _dbContext.TaskLists.AddAsync(taskList);
                await _dbContext.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("/lists/delete/{id}")]
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
