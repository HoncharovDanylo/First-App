import {Injectable} from "@angular/core";
import {CardService} from "../../services/card.service";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {CardsActions} from "./cards-actions-type";
import {catchError, concatMap, map, of} from "rxjs";

@Injectable()
export class CardsEffects{
  constructor(private cardsService : CardService, private action$ : Actions) {}

  loadCards$ = createEffect(() => this.action$.pipe(
    ofType(CardsActions.GetCardsForBoard),
    concatMap((action) => this.cardsService.GetCardsForBoard(action.boardId)),
    map((cards) => CardsActions.CardsLoaded({cards}))
  ))

  createCard$ = createEffect(() => this.action$.pipe(
    ofType(CardsActions.CreateCard),
    concatMap((action) => this.cardsService.CreateCard(action.card)),
    map((card) => CardsActions.CardCreatedSuccess({card})),
    catchError((error) => of(CardsActions.CardCreatedFailure({error})))
  ))

  updateCard$ = createEffect(() => this.action$.pipe(
    ofType(CardsActions.UpdateCard),
    concatMap((action) => this.cardsService.EditCard(Number(action.id), action.update).pipe(
    map(() => CardsActions.CardUpdatedSuccess({update : {id : action.id, changes : {
          title: action.update.Title,
          description: action.update.Description,
          taskListId: action.update.TaskListId,
          priority: action.update.Priority,
          dueDate: action.update.DueDate,
        }}})),
    catchError((error) => of(CardsActions.CardUpdatedFailure({error})
    ))
  ))));

  deleteCard$ = createEffect(() => this.action$.pipe(
    ofType(CardsActions.DeleteCard),
    concatMap((action) => this.cardsService.DeleteCard(action.cardId).pipe(
    map(() => CardsActions.CardDeletedSuccess({cardId : action.cardId})),
    catchError((error) => of(CardsActions.CardDeletedFailure({error})))
  ))))

  moveCard$ = createEffect(() => this.action$.pipe(
    ofType(CardsActions.MoveCard),
    concatMap((action) => this.cardsService.MoveCard(action.card.id, action.listId).pipe(
      map(() => CardsActions.CardMovedSuccess({card : {id : action.card.id, changes : {taskListId : action.listId}}})),
      catchError((error) => of(CardsActions.CardMovedFailure({error})))
    ))
  ))
}
