using Api.Models;

namespace Api.DataInitializers;

public class CardsDataInitializer : DataInitializer<Card>
{
    protected override IList<Card> GetData()
    {
        return new[]
        {
            new Card()
            {
                Id = 1, Title = "Create structure", Description = "This is the first card", Priority = "Low",
                DueDate = DateOnly.Parse("2024-05-01"), TaskListId = 2
            },
            new Card()
            {
                Id = 2, Title = "Setup Entity Framework", Description = "This is the second card", Priority = "High",
                DueDate = DateOnly.Parse("2024-05-10"), TaskListId = 2
            },
            new Card()
            {
                Id = 3, Title = "Create models", Description = "This is the third card", Priority = "Medium",
                DueDate = DateOnly.Parse("2024-05-24"), TaskListId = 2
            },
            new Card()
            {
                Id = 4, Title = "Develop controller for lists", Description = "This is the fourth card",
                Priority = "Medium", DueDate = DateOnly.Parse("2024-06-05"), TaskListId = 2
            },
            new Card()
            {
                Id = 5, Title = "Implement validation", Description = "This is the fifth card", Priority = "Low",
                DueDate = DateOnly.Parse("2024-06-12"), TaskListId = 2
            },
            new Card()
            {
                Id = 6, Title = "Create front-end", Description = "This is the sixth card", Priority = "Low",
                DueDate = DateOnly.Parse("2024-06-14"), TaskListId = 1
            },
            new Card()
            {
                Id = 7, Title = "Test application", Description = "This is the seventh card", Priority = "High",
                DueDate = DateOnly.Parse("2024-06-19"), TaskListId = 1
            },
            new Card()
            {
                Id = 8, Title = "BugFix", Description = "This is the eight card", Priority = "High",
                DueDate = DateOnly.Parse("2024-06-19"), TaskListId = 1
            },
            new Card()
            {
                Id = 9, Title = "Containerize application", Description = "This is the ninths card",
                Priority = "Medium", DueDate = DateOnly.Parse("2024-06-23"), TaskListId = 1
            },
            new Card()
            {
                Id = 10, Title = "Push into master branch", Description = "This is the tenth card", Priority = "Low",
                DueDate = DateOnly.Parse("2024-06-27"), TaskListId = 1
            },
        };
    }
}