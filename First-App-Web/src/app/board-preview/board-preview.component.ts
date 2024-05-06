import {Component, Input, OnInit} from '@angular/core';
import {
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {BoardModel} from "../models/board/board.model";
import {Router} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";
import {Update} from "@ngrx/entity";
import {AppState} from "../../app.state";
import {Store} from "@ngrx/store";
import {BoardActions} from "../store/boards/boards-actions-type";
import {CreateBoardModel} from "../models/board/create-board.model";

@Component({
  selector: 'app-board-preview',
  standalone: true,
  imports: [
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle,
    FormsModule,
    NgIf
  ],
  templateUrl: './board-preview.component.html',
  styleUrl: './board-preview.component.css'
})
export class BoardPreviewComponent implements OnInit{
  @Input() board!: BoardModel;
  ShowEditForm: boolean = false;
  newBoardName: string = '';
  constructor(private router: Router, private store : Store<AppState>) {
  }
  ngOnInit() {
    this.newBoardName = this.board!.name;
  }

  navigateBoard(id: number) {
    this.router.navigate(['board', id])
  }

  onDelete(boardId: number) {
    this.store.dispatch(BoardActions.deleteBoard({boardId}))
  }

  EditBoard() {
    const update : CreateBoardModel= {
        name: this.newBoardName
    }
    this.store.dispatch(BoardActions.boardUpdated({id : this.board.id, update}))
    this.ShowEditForm = false;
  }
}
