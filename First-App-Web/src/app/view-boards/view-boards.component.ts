import {Component, OnInit} from '@angular/core';
import {BoardsService} from "../services/boards.service";
import {Observable} from "rxjs";
import {BoardModel} from "../models/board.model";
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
import {EditBoardModalComponent} from "../edit-board-modal/edit-board-modal.component";
import {DialogModule} from "primeng/dialog";
import {DialogService} from "primeng/dynamicdialog";
import {BoardPreviewComponent} from "../board-preview/board-preview.component";

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
    BoardPreviewComponent
  ],
  providers : [DialogService],
  templateUrl: './view-boards.component.html',
  styleUrl: './view-boards.component.css'
})
export class ViewBoardsComponent implements OnInit {
  Boards: Observable<BoardModel[]> | undefined;
  ShowCreateForm: boolean = false
  newBoardName: string = '';

  constructor(public boardsService: BoardsService, private router: Router, public dialogService: DialogService) {
  }

  ngOnInit(): void {
    this.Boards = this.boardsService.getBoards();
  }
  CreateBoard() {
    this.boardsService.createBoard(this.newBoardName).subscribe({
      next: (response) => {
        this.ShowCreateForm = false;
        this.newBoardName = '';
        this.ngOnInit();
      },
      error: (error) => {
        console.log(error);
      }
    });
  }


}
