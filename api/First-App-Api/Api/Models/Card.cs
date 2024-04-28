using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Card : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    public string Description { get; set; }
    [Required]
    public DateOnly DueDate { get; set; }

    [Required] 
    public bool IsDeleted { get; set; } = false;
    public int? TaskListId { get; set; }

    public TaskList TaskList { get; set; }
    [Required]
    public string Priority { get; set; }
    
}