using Api.Context;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class CardRepository : ICardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<Card?> GetById(int id)
    {
        return await _dbContext.Cards.FindAsync(id);
    }

    public async Task Create(Card card)
    {
        await _dbContext.Cards.AddAsync(card);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Card card)
    {
        _dbContext.Cards.Remove(card);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(Card card)
    {
        _dbContext.Entry(card).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }
    
}