using Api.Context;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BoardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Board?>> GetAll()
    {
        return await _dbContext.Boards.ToListAsync();
    }

    public async Task<Board?> GetById(int id)
    {
        return await _dbContext.Boards.Include(x => x.TaskLists)
            .ThenInclude(x => x.Cards)
            .Include(x => x.Histories).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Create(Board board)
    {
        _dbContext.Boards.Add(board);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Board board)
    {
        _dbContext.Entry(board).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Board board)
    {
        _dbContext.Boards.Remove(board);
        await _dbContext.SaveChangesAsync();
    }
}