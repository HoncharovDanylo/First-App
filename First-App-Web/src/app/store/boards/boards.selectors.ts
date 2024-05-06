import {BoardsState} from "./boards.reducers";
import {createFeatureSelector, createSelector} from "@ngrx/store";
import {state} from "@angular/animations";
import * as fromBoards from './boards.reducers';

export const selectBoardsState
  = createFeatureSelector<BoardsState>('boards');

export const selectAllBoards = createSelector(
  selectBoardsState,
  fromBoards.selectAll);

export const selectBoardById = (boardId: number) => createSelector(
  selectAllBoards,
  boards => boards.find(board => board.id === boardId)
);

export const containsBoard = (boardId: number) => createSelector(
  selectAllBoards,
  boards => {
    return boards.some(board => board.id == boardId)

  });

