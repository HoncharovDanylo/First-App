using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class TaskList : BaseEntity
{
    [Required]
    [Length(5,100)]
    
    public string Name { get; set; }
    public List<Card> Cards { get; set; }
}