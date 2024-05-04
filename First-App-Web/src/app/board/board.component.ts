import {Component, OnInit} from '@angular/core';
import {HistoryGeneralComponent} from "../history-general/history-general.component";
import {ListsComponent} from "../lists/lists.component";
import {SharedModule} from "primeng/api";
import {SidebarModule} from "primeng/sidebar";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {BoardModel} from "../models/board.model";
import {BoardsService} from "../services/boards.service";
import {AsyncPipe, NgIf} from "@angular/common";

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
  Board : Observable<BoardModel> | undefined;
  constructor(private route : ActivatedRoute, private router: Router, private boardsService: BoardsService){
  }

  ngOnInit(): void {
        let id = Number(this.route.snapshot.params['id'])
        this.Board = this.boardsService.getBoardById(id);
        // this.Board.subscribe({
        //   next: (response) => {
        //     console.log(response)
        //   },
        //   error: (error) => {
        //     console.log(error)
        //   }
        //
        // })
    }

  historyClick(){
    this.sidebarVisible = true
  }
}
