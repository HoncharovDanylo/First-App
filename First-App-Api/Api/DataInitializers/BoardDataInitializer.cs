using Api.Models;

namespace Api.DataInitializers;

public class BoardDataInitializer : DataInitializer<Board>
{
    protected override IList<Board> GetData()
    {
        return new[]
        {
            new Board() { Id = 1, Name = "Initial Board" }

        };
    }
}