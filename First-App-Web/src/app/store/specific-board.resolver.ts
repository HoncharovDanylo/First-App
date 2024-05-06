import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {Store} from "@ngrx/store";
import {AppState} from "../../app.state";
import {finalize, first, Observable, tap} from "rxjs";
import {loadAllBoards} from "./boards/boards.action";
import {getTaskListsByBoard} from "./tasklists/tasklists.actions";
import {containsBoard} from "./boards/boards.selectors";
import {BoardActions} from "./boards/boards-actions-type";
import {TaskListsActions} from "./tasklists/tasklists-actions-type";
import {CardsActions} from "./cards/cards-actions-type";

@Injectable()
export class SpecificBoardResolver implements Resolve<any> {
  loading = false;
  wasBoardDownloaded = false;

  constructor(private store: Store<AppState>) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    let id = route.params['id'];
    this.store.select(containsBoard(id)).subscribe((containsBoard) => {
      this.wasBoardDownloaded = containsBoard
    })
    return this.store.pipe(
      tap( ()=> {
        if (!this.loading){
          this.loading = true;
          if (!this.wasBoardDownloaded)
          {
            this.store.dispatch(BoardActions.getBoard({boardId: id}));
          }
          this.store.dispatch(TaskListsActions.getTaskListsByBoard( {boardId:id}));
          this.store.dispatch(CardsActions.GetCardsForBoard({boardId: id}))

        }
      }),
      first(),
      finalize(() => this.loading = false),
    );
  }
}
