namespace Api.Models;

public class Board : BaseEntity
{
    public string Name { get; set; }
    
    public List<TaskList> TaskLists { get; set; }
    public List<History> Histories { get; set; }
}