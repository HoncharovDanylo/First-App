import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {BoardModel} from "../models/board/board.model";
import {Store} from "@ngrx/store";
import {AppState} from "../../app.state";
import {loadAllBoards} from "./boards/boards.action";
import {filter, finalize, first, Observable, tap} from "rxjs";
import {containsBoard} from "./boards/boards.selectors";
import {BoardActions} from "./boards/boards-actions-type";

@Injectable()
export class BoardsResolver implements Resolve<any> {
  loading = false;
  constructor(private store: Store<AppState>) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
    return this.store.pipe(
      tap( ()=> {
        if (!this.loading){
          this.loading = true;
          this.store.dispatch(loadAllBoards());
        }
      }),
      first(),
      finalize(() => this.loading = false),
    );
  }
}

// @Injectable()
// export class SpecificBoardResolver implements Resolve<any> {
//   loading = false;
//   constructor(private store: Store<AppState>) {
//   }
//
//   resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
//     let id = route.params['id'];
//     return this.store.pipe(
//       tap( ()=> {
//         if (!this.loading){
//           let wasBoardDownloaded = false;
//           this.store.select(containsBoard(id)).subscribe((containsBoard) => {
//             wasBoardDownloaded = containsBoard
//           })
//           if (!wasBoardDownloaded)
//           {
//             this.store.dispatch(BoardActions.getBoard({boardId: id}));
//           }
//         }
//       }),
//       first(),
//       finalize(() => this.loading = false),
//     );
//   }
// }
