import {createFeatureSelector, createSelector} from "@ngrx/store";
import {BoardsState} from "../boards/boards.reducers";
import * as fromLists from "../tasklists/tasklists.reducers";
import {ListsState} from "./tasklists.reducers";
import {selectAllBoards} from "../boards/boards.selectors";

export const selectListsState
  = createFeatureSelector<ListsState>('lists');

export const selectAllLists = createSelector(
  selectListsState,
  fromLists.selectAll);

export const selectListsByBoard = (boardId: number) => createSelector(
  selectAllLists,
  lists => {
    return lists.filter(list => list.boardId == boardId)
  });

export const GetListsForMove=  (boardId : number, listId : number) => createSelector(
  selectAllLists,
  lists => {
    return lists.filter(list => list.boardId == boardId && list.id != listId)});

export const GetListsForMoveWithCurrent = (boardId : number, listId : number) => createSelector(
  selectAllLists,
  lists=>{
 return lists.filter(list => list.boardId == boardId)
   .sort(function(x,y){ return x.id == listId ? -1 : y.id == listId ? 1 : 0; });
  });


