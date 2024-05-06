import {Component, OnInit} from '@angular/core';
import {HistoryGeneralComponent} from "../history-general/history-general.component";
import {ListsComponent} from "../lists/lists.component";
import {SharedModule} from "primeng/api";
import {SidebarModule} from "primeng/sidebar";
import {ActivatedRoute} from "@angular/router";
import {Observable} from "rxjs";
import {BoardModel} from "../models/board/board.model";
import {AsyncPipe, NgIf} from "@angular/common";
import {AppState} from "../../app.state";
import {Store} from "@ngrx/store";
import {selectBoardById} from "../store/boards/boards.selectors";

@Component({
  selector: 'app-board',
  standalone: true,
  imports: [
    HistoryGeneralComponent,
    ListsComponent,
    SharedModule,
    SidebarModule,
    AsyncPipe,
    NgIf
  ],
  templateUrl: './board.component.html',
  styleUrl: './board.component.css'
})
export class BoardComponent implements OnInit{
  sidebarVisible = false
  Board! : Observable<BoardModel | undefined>
  constructor(private route : ActivatedRoute, private store : Store<AppState>){}

  ngOnInit(): void {
        let id = Number(this.route.snapshot.params['id'])
        this.Board = this.store.select(selectBoardById(id))!
    }
  historyClick(){
    this.sidebarVisible = true
  }
}
