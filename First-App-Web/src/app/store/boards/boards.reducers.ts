import {BoardModel} from "../../models/board/board.model";
import {createEntityAdapter, EntityState} from "@ngrx/entity";
import {createReducer, on} from "@ngrx/store";
import {BoardActions} from "./boards-actions-type";

export interface BoardsState extends EntityState<BoardModel>{
}

export const adapter = createEntityAdapter<BoardModel>();
export const  initialBoardsState = adapter.getInitialState();
export const boardsReducer = createReducer(
  initialBoardsState,
  on(BoardActions.allBoardsLoaded,
    (state, action) =>
      adapter.setAll(action.boards, state)),

  on(BoardActions.boardUpdated,
    (state, action) =>{return state}),
  on(BoardActions.boardUpdateSuccess,
    (state, action) =>
      adapter.updateOne(action.update, state)),
  on(BoardActions.boardUpdateFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(BoardActions.getBoard,
    (state, action) => {return state}),
  on(BoardActions.boardLoaded,
    (state, action) => adapter.addOne(action.board, state)),
  on(BoardActions.boardLoadFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(BoardActions.createBoard,
    (state, action) => {return state}),
  on(BoardActions.boardCreatedSuccess,
    (state, action) =>
      adapter.addOne(action.board, state)),
  on(BoardActions.boardCreatedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),


  on(BoardActions.deleteBoard,
    (state, action) => {return state}),
  on(BoardActions.boardDeletedSuccess,
    (state,action) => adapter.removeOne(action.boardId, state)),
  on(BoardActions.boardDeletedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  })
)

export const {selectAll} = adapter.getSelectors();

