import {createAction, props} from "@ngrx/store";
import {BoardModel} from "../../models/board/board.model";
import {Update} from "@ngrx/entity";
import {CreateBoardModel} from "../../models/board/create-board.model";

export const loadAllBoards = createAction(
  '[Boards Resolver] Load All Boards'
);

export const allBoardsLoaded = createAction(
  '[Load Boards Effect] All Boards Loaded',
  props<{boards : BoardModel[]}>()
);

export const getBoard = createAction(
  '[Board Component] Get Board',
  props<{boardId : number}>()
)

export const boardLoaded = createAction(
  '[Get Board Effect] Board Loaded',
  props<{board : BoardModel}>()

)
export const boardLoadFailure = createAction(
  '[Get Board Effect] Board Load Failure',
  props<{error : any}>()

)

export const boardUpdated = createAction(
  '[Update Board Form] Board Updated',
  props<{id: number,update : CreateBoardModel}>()
);
export const boardUpdateSuccess = createAction(
  '[Update Board Effect] Board Updated Success',
  props<{update : Update<BoardModel>}>()
);
export const boardUpdateFailure = createAction(
  '[Update Board Effect] Board Updated Failure',
  props<{error : any}>()
);


export const createBoard = createAction(
  '[Create Board Form] Create Board',
  props<{name : CreateBoardModel}>()
)
export const boardCreatedSuccess = createAction(
  '[Create Board Effect] Board Created',
  props<{board : BoardModel}>()
);
export const boardCreatedFailure = createAction(
  '[Create Board Effect] Board Created Failure',
  props<{error : any}>()
);
export const deleteBoard = createAction(
  '[Board Preview Component] Delete Board',
  props<{boardId : number}>()
)
export const boardDeletedSuccess = createAction(
  '[Delete Board Effect] Board Deleted Success',
  props<{boardId : number}>()
)
export const boardDeletedFailure = createAction(
  '[Delete Board Effect] Board Deleted Failure',
  props<{error : any}>()
)

