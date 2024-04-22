using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Card : BaseEntity
{
    [Required]
    [Length(5,100)]
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    [Required]
    public int TaskListId { get; set; }

    public TaskList TaskList { get; set; }
    public string Priority { get; set; }
}