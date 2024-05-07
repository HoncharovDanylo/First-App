using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Api.Context;
using Api.Interfaces;
using Api.Models;
using Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IntegrationTests.UnitTesting;

public class BoardRepositoryTests
{
    private ApplicationDbContext _dbMock;
    private List<Board> _boards;
    public BoardRepositoryTests()
    {
        _boards = BoardsObjects.GetBoards();
        IQueryable<Board> data = _boards.AsQueryable();
        Mock<DbSet<Board>> dbSetMock = new Mock<DbSet<Board>>();
        Mock<ApplicationDbContext> dbContext = new Mock<ApplicationDbContext>();
        dbSetMock.As<IQueryable<Board>>().Setup(s => s.Provider).Returns(data.Provider);
        dbSetMock.As<IQueryable<Board>>().Setup(s => s.Expression).Returns(data.Expression);
        dbSetMock.As<IQueryable<Board>>().Setup(s => s.ElementType).Returns(data.ElementType);
        dbSetMock.As<IQueryable<Board>>().Setup(s => s.GetEnumerator()).Returns(() => data.GetEnumerator());
        dbSetMock.Setup(x => x.Add(It.IsAny<Board>())).Callback<Board>(_boards.Add);
        dbSetMock.Setup(x => x.Remove(It.IsAny<Board>())).Callback<Board>(t => _boards.Remove(t));
        dbSetMock.Setup(x => x.Update(It.IsAny<Board>())).Callback<Board>(t =>
        {
            _boards.RemoveAll(b => t.Id == b.Id);
            _boards.Add(t);
        });
        dbSetMock.As<IQueryable<Board>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        dbContext.Setup<DbSet<Board>>(x => x.Boards).Returns(dbSetMock.Object);
        dbContext.Setup(context => context.Boards ).Returns(dbSetMock.Object);
        
        _dbMock = dbContext.Object;
    }

    [Fact]
    public async Task CreateBoard_ModelIsValid_BoardCreated()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        var board = new Board()
        {
            Id = 5,
            Name = "Board5"
        };
        
        //Act
        await boardRepository.Create(board);
        
        //Assert
        Assert.Equal(5, _boards.Count);
         
        
        
    }
    [Fact]
    public async Task GetById_IdIsInvalid_ReturnsNull()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        
        //Act
        var board = await boardRepository.GetById(10);
        
        //Assert
        Assert.Null(board);
    }
    [Fact]
    public async Task GetById_IdIsValid_ReturnsBoard()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        
        //Act
        var board = await boardRepository.GetById(2);
        
        //Assert
        Assert.NotNull(board);
        Assert.Equal(2, board.Id);
        Assert.Equal("Board2", board.Name);
    }
    [Fact]
    public async Task GetAll_ReturnsAllBoards()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        
        //Act
        var boards = await boardRepository.GetAll();
        
        //Assert
        Assert.NotNull(boards);
        Assert.Equal(4, boards.Count());
    }
    
    [Fact]
    public async Task UpdateBoard_BoardUpdated()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        var board = _boards.Find(x => x.Id == 2);
        board.Name = "Board2Updated";
        
        //Act
        await boardRepository.Update(board);
        
        //Assert
        Assert.Equal("Board2Updated", _boards.FirstOrDefault(x=>x.Id == 2)?.Name);
    }
    
    [Fact]
    public async Task DeleteBoard_BoardDeleted()
    {
        //Arrange
        var boardRepository = new BoardRepository(_dbMock);
        var board = _boards.Find(x => x.Id == 2);
        
        //Act
        await boardRepository.Delete(board);
        
        //Assert
        Assert.Null(_boards.FirstOrDefault(x=>x.Id == 2));
    }
}