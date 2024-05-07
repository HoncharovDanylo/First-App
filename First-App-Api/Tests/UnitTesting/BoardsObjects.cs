using Api.Models;

namespace IntegrationTests.UnitTesting;

public class BoardsObjects
{
    public static List<Board> GetBoards()
    {
        return new()
        {
            new()
            {
                Id = 1,
                Name = "Board1"
            },
            new()
            {
                Id = 2,
                Name = "Board2"
            },
            new()
            {
                Id = 3,
                Name = "Board3"
            },
            new()
            {
                Id = 4,
                Name = "Board4"
            },
            
        };
    }
}