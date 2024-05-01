using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Card : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    
    public int TaskListId { get; set; }

    public TaskList TaskList { get; set; }
    public string Priority { get; set; }
    
}