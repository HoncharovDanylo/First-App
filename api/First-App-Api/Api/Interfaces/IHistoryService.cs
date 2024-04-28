using Api.Models;
using Api.Models.DTOs.CardDTOs;

namespace Api.Interfaces;

public interface IHistoryService
{
    Task TrackCreation(Card card);
    Task TrackDeletion(Card card);
    Task TrackUpdate(Card oldCard, CreateUpdateCardDto updatedCard);
    Task TrackMove(Card card, int newTaskListId);
}