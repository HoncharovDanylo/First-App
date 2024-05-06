import {Component, OnInit} from '@angular/core';
import {BoardsService} from "../services/boards.service";
import {Observable} from "rxjs";
import {BoardModel} from "../models/board/board.model";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {Router} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {DialogModule} from "primeng/dialog";
import {DialogService} from "primeng/dynamicdialog";
import {BoardPreviewComponent} from "../board-preview/board-preview.component";
import {EffectsModule} from "@ngrx/effects";
import {BoardsEffects} from "../store/boards/boards.effects";
import {AppState} from "../../app.state";
import {select, Store} from "@ngrx/store";
import {selectAllBoards} from "../store/boards/boards.selectors";
import {createBoard} from "../store/boards/boards.action";

@Component({
  selector: 'app-view-boards',
  standalone: true,
  imports: [
    AsyncPipe,
    NgForOf,
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle,
    NgIf,
    FormsModule,
    DialogModule,
    BoardPreviewComponent,
  ],
  providers : [DialogService],
  templateUrl: './view-boards.component.html',
  styleUrl: './view-boards.component.css'
})
export class ViewBoardsComponent implements OnInit {
  Boards: Observable<BoardModel[]> | undefined;
  ShowCreateForm: boolean = false
  newBoardName: string = '';

  constructor(public boardsService: BoardsService, private router: Router, public dialogService: DialogService, private store: Store<AppState>) {
  }

  ngOnInit(): void {
    this.Boards = this.store.pipe(select(selectAllBoards));
  }
  CreateBoard() {
    this.store.dispatch(createBoard({name: {name : this.newBoardName}}));
    this.ShowCreateForm = false;
    this.newBoardName = '';
  }


}
