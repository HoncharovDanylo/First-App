namespace Api.Models;

public class Card : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int TaskListId { get; set; }
    public int PriorityId { get; set; }

    public TaskList TaskList { get; set; }
    public TasksPriority Priority { get; set; }
}