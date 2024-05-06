import {CardsState} from "./cards.reducers";
import * as fromCards from './cards.reducers';
import {createFeatureSelector, createSelector} from "@ngrx/store";
import {selectAllLists} from "../tasklists/tasklists.selectors";
import {CardsActions} from "./cards-actions-type";

export const selectCardsState = createFeatureSelector<CardsState>('cards');

 export const selectAllCards = createSelector(
   selectCardsState,
   fromCards.selectAll);

  export const selectCardsByList = (listId: number) => createSelector(
    selectAllCards,
    cards => {
      return cards.filter(card => card.taskListId == listId)
    });

  export const getCardById = (cardId: number) => createSelector(
    selectAllCards,
    cards => cards.find(x=>x.id == cardId)
  )

export const GetCountOfCardsByList = (listId: number) => createSelector(
  selectCardsByList(listId),
  cards => cards.filter(card => card.taskListId == listId ).length
)
