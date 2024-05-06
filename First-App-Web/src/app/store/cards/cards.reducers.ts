import {createEntityAdapter, EntityState} from "@ngrx/entity";
import {ListModel} from "../../models/tasklist/list.model";
import {CardModel} from "../../models/card/card.model";
import {createReducer, on} from "@ngrx/store";
import {CardsActions} from "./cards-actions-type";
import {act} from "@ngrx/effects";

export interface CardsState extends EntityState<CardModel>{}

export const adapter = createEntityAdapter<CardModel>();
export const  initialCardsState = adapter.getInitialState();

export const cardsReducer = createReducer(
  initialCardsState,
  on(CardsActions.GetCardsForBoard,
    (state, action) => {return state}),
  on(CardsActions.CardsLoaded,
    (state, action) =>
      adapter.addMany(action.cards, state)),

  on(CardsActions.CreateCard,
    (state, action) => {return state}),
  on(CardsActions.CardCreatedSuccess,
    (state, action) =>
      adapter.addOne(action.card, state)),
  on(CardsActions.CardCreatedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(CardsActions.UpdateCard,
    (state, action) => {return state} ),
  on(CardsActions.CardUpdatedSuccess,
    (state, action) =>
      adapter.updateOne(action.update,  state)),
  on(CardsActions.CardUpdatedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(CardsActions.DeleteCard,
    (state, action) => {return state}),
  on(CardsActions.CardDeletedSuccess,
    (state, action) => adapter.removeOne(action.cardId, state)),
  on(CardsActions.CardDeletedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(CardsActions.MoveCard,
    (state, action) => {return state}),
  on(CardsActions.CardMovedSuccess,
    (state, action) => adapter.updateOne(action.card, state)),
  on(CardsActions.CardMovedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

)

export const {selectAll} = adapter.getSelectors();
