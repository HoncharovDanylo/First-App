using System.Security.Cryptography;
using Api.Context;
using Api.Interfaces;
using Api.Models;
using Api.Models.DTOs.CardDTOs;

namespace Api.Services;

public class HistoryService : IHistoryService
{
    
    public History TrackCreation(Card card, string listName)
    {
        var history = new History()
        {
            Action = "Create",
            CardId = card.Id,
            Field = "Card",
            NewValue = card.Title,
            Timestamp = DateTime.UtcNow,
            CardTitle = card.Title,
            ListName = listName
        };
        return history;
    }

    public History TrackDeletion(Card card, string listName)
    {
        var history = new History()
        {
            Action = "Delete",
            Field = "Card",
            PreviousValue = card.Title,
            Timestamp = DateTime.UtcNow,
            CardId = card.Id,
            CardTitle = card.Title,
            ListName = listName
        };
        return history;
    }

    public IEnumerable<History> TrackUpdate(Dictionary<string,object> oldValue, Dictionary<string,object> newValue)
    {
        var changes = new List<History>();
        if(!oldValue["Title"].Equals(newValue["Title"]))
            changes.Add(new()
            {
                Action = "Change",
                Field = "Title",
                PreviousValue = oldValue["Title"].ToString(),
                NewValue = newValue["Title"].ToString(),
                Timestamp = DateTime.UtcNow,
                CardId = (int)oldValue["Id"],
                CardTitle = newValue["Title"].ToString()

            });
        if(!oldValue["Description"].Equals(newValue["Description"]))
            changes.Add(new()
            {
                Action = "Change",
                Field = "Description",
                PreviousValue = oldValue["Description"].ToString(),
                NewValue = newValue["Description"].ToString(),
                Timestamp = DateTime.UtcNow,
                CardId = (int)oldValue["Id"],
                CardTitle = newValue["Title"].ToString()

            });
        if(!oldValue["DueDate"].Equals(newValue["DueDate"]))
            changes.Add(new()
            {
                Action = "Change",
                Field = "Due Date",
                PreviousValue = oldValue["DueDate"].ToString(),
                NewValue = newValue["DueDate"].ToString(),
                Timestamp = DateTime.UtcNow,
                CardId = (int)oldValue["Id"],
                CardTitle = newValue["Title"].ToString()

            });
        if(!oldValue["TaskListId"].Equals(newValue["TaskListId"]))
            changes.Add(new()
            {
                Action = "Change",
                Field = "Task List",
                PreviousValue = oldValue["oldListName"].ToString(),
                NewValue = newValue["newListName"].ToString(),
                Timestamp = DateTime.UtcNow,
                CardId = (int)oldValue["Id"],
                CardTitle = newValue["Title"].ToString()

            });
        if(!oldValue["Priority"].Equals(newValue["Priority"]))
            changes.Add(new()
            {
                Action = "Change",
                Field = "Priority",
                PreviousValue = oldValue["Priority"].ToString(),
                NewValue = newValue["Priority"].ToString(),
                Timestamp = DateTime.UtcNow,
                CardId = (int)oldValue["Id"],
                CardTitle = newValue["Title"].ToString()

            });
        return changes;
    }

}