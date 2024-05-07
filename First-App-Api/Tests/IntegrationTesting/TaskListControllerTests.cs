using System.Net;
using System.Net.Http.Json;
using Api.Models.DTOs.BoardDTOs;
using Api.Models.DTOs.TaskListDTOs;
using FluentAssertions;

namespace IntegrationTests;

public class TaskListControllerTests
{
    [Fact]
    public async Task CreateList_BoardIdIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();

        var board = new CreateUpdateBoardDto()
        {
            Name = "Testing Board"
        };
        var list = new CreateTaskListDto()
        {
            BoardId = 2, // Invalid Param
            Name = "TestingList"
        };

        await client.PostAsJsonAsync("/boards/create", board);
        
        //Act
        var response  = await client.PostAsJsonAsync("/lists/create", list);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task CreateList_NameIsInvalid_ReturnsBadRequest()
    {
        //Arrange
        
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();

        var board = new CreateUpdateBoardDto()
        {
            Name = "Testing Board"
        };
        var list = new CreateTaskListDto()
        {
            BoardId = 1,
            Name = null
        };
        
        //Act
        
        await client.PostAsJsonAsync("/boards/create", board);
        var response  = await client.PostAsJsonAsync("/lists/create", list);

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async Task CreateList_ModelIsValid_ReturnsOk()
    {
        //Arrange
        var app = new FirstApplicationFactory();
        var client = app.CreateClient();

        var board = new CreateUpdateBoardDto()
        {
            Name = "Testing Board"
        };
        var list = new CreateTaskListDto()
        {
            BoardId = 1,
            Name = "SomeName"
        };

        //Act
        await client.PostAsJsonAsync("/boards/create", board);
        var response  = await client.PostAsJsonAsync("/lists/create", list);
        
        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}