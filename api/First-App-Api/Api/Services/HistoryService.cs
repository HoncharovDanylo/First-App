using System.Security.Cryptography;
using Api.Context;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;

namespace Api.Services;

public class HistoryService : IHistoryService
{
    private readonly ApplicationDbContext _dbContext;

    public HistoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task TrackCreation(Card card)
    {
        var history = new History()
        {
            Action = "Create",
            CardId = card.Id,
            Field = "Card",
            NewValue = card.Title,
            Timestamp = DateTime.Now.ToUniversalTime(),
        };
        await _dbContext.Histories.AddAsync(history);
        await _dbContext.SaveChangesAsync();
    }

    public async Task TrackDeletion(Card card)
    {
        var history = new History()
        {
            Action = "Delete",
            Field = "Card",
            PreviousValue = card.Title,
            Timestamp = DateTime.Now.ToUniversalTime(),
        };
        await _dbContext.Histories.AddAsync(history);
        await _dbContext.SaveChangesAsync();
    }

    public async Task TrackUpdate(Card oldCard, CreateUpdateCardDto updatedCard)
    {
        var changes = new List<History>();
        if(oldCard.Title != updatedCard.Title)
            changes.Add(new()
            {
                Action = "Change",
                Field = "Title",
                PreviousValue = oldCard.Title,
                NewValue = updatedCard.Title,
                Timestamp = DateTime.Now.ToUniversalTime(),
                CardId = oldCard.Id
            });
        if(oldCard.Description != updatedCard.Description)
            changes.Add(new()
            {
                Action = "Change",
                Field = "Description",
                PreviousValue = oldCard.Description,
                NewValue = updatedCard.Description,
                Timestamp = DateTime.Now.ToUniversalTime(),
                CardId = oldCard.Id
            });
        if(oldCard.DueDate != updatedCard.DueDate)
            changes.Add(new()
            {
                Action = "Change",
                Field = "Due Date",
                PreviousValue = oldCard.DueDate.ToString(),
                NewValue = updatedCard.DueDate.ToString(),
                Timestamp = DateTime.Now.ToUniversalTime(),
                CardId = oldCard.Id
            });
        if(oldCard.TaskListId != updatedCard.TaskListId)
            changes.Add(new()
            {
                Action = "Change",
                Field = "Task List",
                PreviousValue = oldCard.TaskListId.ToString(),
                NewValue = updatedCard.TaskListId.ToString(),
                Timestamp = DateTime.Now,
                CardId = oldCard.Id   
            });
        if(oldCard.Priority != updatedCard.Priority)
            changes.Add(new()
            {
                Action = "Change",
                Field = "Priority",
                PreviousValue = oldCard.Priority,
                NewValue = updatedCard.Priority,
                Timestamp = DateTime.Now.ToUniversalTime(),
                CardId = oldCard.Id 
            });
        await _dbContext.Histories.AddRangeAsync(changes);
        await _dbContext.SaveChangesAsync();
    }

    public async Task TrackMove(Card card, int newTaskListId)
    {
        var history = new History()
        {
            Action = "Change",
            Field = "Task List",
            PreviousValue = card.TaskListId.ToString(),
            NewValue = newTaskListId.ToString(),
            Timestamp = DateTime.Now.ToUniversalTime(),
            CardId = card.Id
        };
        await _dbContext.Histories.AddAsync(history);
        await _dbContext.SaveChangesAsync();
    }
}