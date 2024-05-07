using Api.Models;

namespace Api.DataInitializers;

public class TaskListsDataInitializer : DataInitializer<TaskList>
{
    protected override IList<TaskList> GetData()
    {
        return new[]
        {
            new TaskList() { Id = 1, Name = "To Do", BoardId = 1 },
            new TaskList() { Id = 2, Name = "In progress", BoardId = 1 },
        };
    }
}