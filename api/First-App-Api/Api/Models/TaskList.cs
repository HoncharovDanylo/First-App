namespace Api.Models;

public class TaskList : BaseEntity
{
    public string Name { get; set; }
    public List<Card> Card { get; set; }
}