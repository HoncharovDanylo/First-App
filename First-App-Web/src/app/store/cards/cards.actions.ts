import {createAction, props} from "@ngrx/store";
import {CardModel} from "../../models/card/card.model";
import {CreateCardModel} from "../../models/card/create-card.model";
import {Update} from "@ngrx/entity";

export const GetCardsForBoard = createAction(
  '[Cards Resolver] Get Cards For Board',
        props<{boardId : number}>()
)
export const CardsLoaded = createAction(
  '[Get Cards Effect] Cards Loaded',
  props<{cards : CardModel[]}>()
)

export const CreateCard = createAction(
  '[Create Card Form] Create Card',
  props<{card : CreateCardModel}>()
)
export const CardCreatedSuccess = createAction(
  '[Create Card Effect] Card Created Success',
  props<{card : CardModel}>()
)
export const CardCreatedFailure = createAction(
  '[Create Card Effect] Card Created Failure',
  props<{error : any}>()
)

export const UpdateCard = createAction(
  '[Update Card Form] Update Card',
  props<{update : CreateCardModel, id : number}>()
)
export const CardUpdatedSuccess = createAction(
  '[Update Card Effect] Card Updated Success',
  props<{update : Update<CardModel>}>()
)
export const CardUpdatedFailure = createAction(
  '[Update Card Effect] Card Updated Failure',
  props<{error : any}>()
)

export const MoveCard = createAction(
  '[Card Component] Move Card',
  props<{card : CardModel, listId : number}>()
)
export const CardMovedSuccess = createAction(
  '[Move Card Effect] Card Moved Success',
  props<{card : Update<CardModel>}>()
)
export const CardMovedFailure = createAction(
  '[Move Card Effect] Card Moved Failure',
  props<{error : any}>()
)

export const DeleteCard = createAction(
  '[Card Component] Delete Card',
  props<{cardId : number}>()
)
export const CardDeletedSuccess = createAction(
  '[Delete Card Effect] Card Deleted Success',
  props<{cardId : number}>()
)

export const CardDeletedFailure = createAction(
  '[Delete Card Effect] Card Deleted Failure',
  props<{error : any}>()
)
