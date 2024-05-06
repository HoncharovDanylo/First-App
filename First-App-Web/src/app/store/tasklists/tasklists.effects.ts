import {Injectable} from "@angular/core";
import {BoardsService} from "../../services/boards.service";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {BoardActions} from "../boards/boards-actions-type";
import {catchError, concatMap, map, of} from "rxjs";
import {BoardModel} from "../../models/board/board.model";
import {ListService} from "../../services/list.service";
import {TaskListsActions} from "./tasklists-actions-type";
import {SpecificBoardResolver} from "../specific-board.resolver";
import {ListModel} from "../../models/tasklist/list.model";

@Injectable()
export class ListsEffects {
  constructor(private listService : ListService, private actions$ : Actions) {
  }
  loadBoards$ = createEffect(() => this.actions$.pipe(
    ofType(TaskListsActions.getTaskListsByBoard),
    concatMap((action) => this.listService.GetLists(action.boardId)),
    map((lists : ListModel[]) => TaskListsActions.taskListsLoaded({taskLists : lists})
    )));

  createTaskList$ = createEffect(() => this.actions$.pipe(
    ofType(TaskListsActions.createTaskList),
    concatMap((action) => this.listService.CreateList({name : action.name, boardId:action.boardId}).pipe
    (map((taskList : ListModel) => TaskListsActions.taskListCreatedSuccess({taskList})),
      catchError((error) => of(TaskListsActions.taskListCreatedFailure({error}))),
    ))));

  deleteTaskList$ = createEffect(() => this.actions$.pipe(
    ofType(TaskListsActions.deleteTaskList),
    concatMap((action) => this.listService.DeleteList(action.listId).pipe(
      map(() => TaskListsActions.taskListDeletedSuccess({listId : action.listId})),
      catchError((error) => of(TaskListsActions.taskListDeletedFailure({error})
        )
      )))));

  updateTaskList$ = createEffect(() => this.actions$.pipe(
    ofType(TaskListsActions.updateTaskList),
    concatMap((action) => this.listService.EditListName(action.list, action.listId).pipe(
      map(() => TaskListsActions.taskListUpdatedSuccess({update : {
        id : action.listId,
        changes : {
          name:  action.list.name
        }}})),
      catchError((error) => of(TaskListsActions.taskListUpdatedFailure({error})
        )
      )))));

}
