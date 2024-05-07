using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Models;
using Api.Models.DTOs.BoardDTOs;
using Api.Models.DTOs.CardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using FluentAssertions;

namespace IntegrationTests;

public class CardControllerTests
{
    [Fact]
    public async Task CreateCard_DueDateIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(-1), // Invalid Parameter,
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }
    
    [Fact]
    public async Task CreateCard_TitleIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "", // Invalid Parameter
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }
    [Fact]
    public async Task CreateCard_PriorityIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "SomePriority" // Invalid Parameter
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

    }
    [Fact]
    public async Task CreateCard_TaskListIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 34, // Invalid list id
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateCard_TaskListIsIEmpty_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task CreateCard_ModelIsValid_ReturnsOk()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        
        //Act
        var response = await client.PostAsJsonAsync("/cards/create", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task MoveCard_MoveToListFromOtherBoard_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board1 = new CreateUpdateBoardDto(){ Name = "Testing Board1" };
        var taskList1 = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var board2 = new CreateUpdateBoardDto(){ Name = "Testing Board2" };
        var taskList2 = new CreateTaskListDto(){ Name = "Test List2", BoardId = 2};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board1);
        await client.PostAsJsonAsync("/boards/create", board2);
        await client.PostAsJsonAsync("/lists/create", taskList1);
        await client.PostAsJsonAsync("/lists/create", taskList2);
        await client.PostAsJsonAsync("/cards/create", card);

        var response = await client.PatchAsJsonAsync("/card/1/change-list/2",new object());
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task MoveCard_ListDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);
        
        //Act
        var response = await client.PatchAsJsonAsync("/card/1/change-list/10", new object());
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task MoveCard_CardDoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);
        
        //Act
        var response = await client.PatchAsJsonAsync("/card/10/change-list/1", new object());
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task MoveCard_ListFromTheSameBoard_ReturnsOk()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList1 = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var taskList2 = new CreateTaskListDto(){ Name = "Test List2", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList1);
        await client.PostAsJsonAsync("/lists/create", taskList2);
        await client.PostAsJsonAsync("/cards/create", card);
        
        //Act
        var response = await client.PatchAsJsonAsync("/card/1/change-list/2", card);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteCard_CardIdIsCorrect_ReturnsOk()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);
        
        //Act
        var response = await client.DeleteAsync($"cards/delete/1");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    [Fact]
    public async Task DeleteCard_CardIdIsNotCorrect_ReturnsNotFound()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);
        
        //Act
        var response = await client.DeleteAsync($"cards/delete/2");
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task EditCard_IdIsInvalid_ReturnsNotFound()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);

        var cardEdit = new CreateUpdateCardDto()
        {
            Title = "UpdatedCard",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        //Act
        var response = await client.PutAsJsonAsync("cards/update/2", cardEdit);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task EditCard_ModelIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);

        var cardEdit = new CreateUpdateCardDto()
        {
            Title = "UpdatedCard",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(-1),
            TaskListId = 4,
            Priority = "SomeOther"
        };
        //Act
        var response = await client.PutAsJsonAsync("cards/update/1", cardEdit);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest); 
    }
    [Fact]
    public async Task EditCard_ModelIsValid_ReturnsOk()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();
        var board = new CreateUpdateBoardDto(){ Name = "Testing Board" };
        var taskList = new CreateTaskListDto(){ Name = "Test List1", BoardId = 1};
        var card = new CreateUpdateCardDto()
        {
            Title = "Testing card",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        await client.PostAsJsonAsync("/boards/create", board);
        await client.PostAsJsonAsync("/lists/create", taskList);
        await client.PostAsJsonAsync("/cards/create", card);

        var cardEdit = new CreateUpdateCardDto()
        {
            Title = "UpdatedCard",
            Description = "Testing description",
            DueDate = DateTime.Now.AddDays(1),
            TaskListId = 1,
            Priority = "Low"
        };
        //Act
        var response = await client.PutAsJsonAsync("cards/update/1", cardEdit);
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
}