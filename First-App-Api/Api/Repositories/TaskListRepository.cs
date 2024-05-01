using Api.Context;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class TaskListRepository : ITaskListRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskListRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<TaskList?>> GetTaskLists()
    {
        return await _dbContext.TaskLists.Include(x=>x.Cards).ToListAsync();
    }

    public async Task<TaskList?> GetById(int id)
    {
       return await _dbContext.TaskLists.Include(x=>x.Cards).FirstOrDefaultAsync(x=>x.Id==id);
    }

    public async Task Create(TaskList taskList)
    {
        await _dbContext.AddAsync(taskList);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(TaskList list)
    {
        _dbContext.Remove(list);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(TaskList taskList)
    {
        _dbContext.Entry(taskList).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

}