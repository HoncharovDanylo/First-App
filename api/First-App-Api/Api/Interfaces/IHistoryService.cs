using Api.Models;
using Api.Models.DTOs.CardDTOs;

namespace Api.Interfaces;

public interface IHistoryService
{
    History TrackCreation(Card card);
    History TrackDeletion(Card card);
    IEnumerable<History> TrackUpdate(Dictionary<string,object> oldValue, Dictionary<string,object> newValue);
    // Task TrackMove(Card card, int newTaskListId);
}