using System.ComponentModel.DataAnnotations;

namespace Api.Models;
public class TaskList : BaseEntity
{
    
  
    public string Name { get; set; }
    
    public int BoardId { get; set; }
    public Board Board { get; set; }
    public List<Card> Cards { get; set; }
}