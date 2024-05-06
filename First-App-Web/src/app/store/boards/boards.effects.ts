import {Injectable} from "@angular/core";
import {Actions, createEffect, ofType} from "@ngrx/effects";
import {BoardActions} from "./boards-actions-type";
import {catchError, concatMap, map, of} from "rxjs";
import {BoardsService} from "../../services/boards.service";
import {BoardsResolver} from "../boards.resolver";
import {BoardModel} from "../../models/board/board.model";
import {Update} from "@ngrx/entity";
import {CreateBoardModel} from "../../models/board/create-board.model";

@Injectable()
export class BoardsEffects {
  constructor(private boardService : BoardsService, private actions$ : Actions) {
  }
 loadBoards$ = createEffect(() => this.actions$.pipe(
   ofType(BoardActions.loadAllBoards),
   concatMap((action) => this.boardService.getBoards()),
   map((boards : BoardModel[]) => BoardActions.allBoardsLoaded({boards})
 )));

  getBoard$ = createEffect(() => this.actions$.pipe(
    ofType(BoardActions.getBoard),
    concatMap((action) => this.boardService.getBoardById(action.boardId)),
    map((board : BoardModel) => BoardActions.boardLoaded({board})),
    catchError((error) => of(BoardActions.boardLoadFailure({error})))
  ))

  updateBoard$ = createEffect(() => this.actions$.pipe(
    ofType(BoardActions.boardUpdated),
    concatMap((action) => this.boardService.editBoard(action.id, action.update).pipe(
      map(() => BoardActions.boardUpdateSuccess({update : {
        id : action.id,
        changes : {
        name:  action.update.name
        }
        }})),
      catchError((error) => of(BoardActions.boardUpdateFailure({error})))
    ))

  ));

  createBoard$ = createEffect(() => this.actions$.pipe(
    ofType(BoardActions.createBoard),
    concatMap((action) => this.boardService.createBoard(action.name).pipe
    (map((board : BoardModel) => BoardActions.boardCreatedSuccess({board})),
      catchError((error) => of(BoardActions.boardCreatedFailure({error}))),
  ))));

  deleteBoard$ = createEffect(() => this.actions$.pipe(
    ofType(BoardActions.deleteBoard),
      concatMap((action) => this.boardService.deleteBoard(action.boardId).pipe(
        map(() => BoardActions.boardDeletedSuccess({boardId : action.boardId})),
        catchError((error) => of(BoardActions.boardDeletedFailure({error})
      )
  )))))

}
