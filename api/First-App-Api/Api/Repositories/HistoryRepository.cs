
using Api.Context;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly ApplicationDbContext _dbContext;

    public HistoryRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<History>> GetAll(int page)
    {
        return  await _dbContext.Histories.OrderByDescending(x=>x.Timestamp).Skip((page-1) * 20).Take(20).ToListAsync();
    }

    public async Task<IEnumerable<History>> GetByCardId(int cardId)
    {
        return  await _dbContext.Histories.OrderByDescending(x=>x.Timestamp).Where(x=>x.CardId==cardId).ToListAsync();

    }
}