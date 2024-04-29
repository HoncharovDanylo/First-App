using Api.Models;

namespace Api.Interfaces;

public interface ITaskListRepository
{
    Task<IEnumerable<TaskList?>> GetTaskLists();
    Task<TaskList?> GetById(int id);
    Task Create(TaskList taskList);
    Task Delete(TaskList list);
    Task Update(TaskList taskList);
}