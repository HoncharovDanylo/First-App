import {Component, Input, OnInit} from '@angular/core';
import {
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {BoardModel} from "../models/board.model";
import {BoardsService} from "../services/boards.service";
import {Router} from "@angular/router";
import {FormsModule} from "@angular/forms";
import {NgIf} from "@angular/common";

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
  constructor(private router: Router, private boardsService: BoardsService) {
  }
  ngOnInit() {
    this.newBoardName = this.board!.name;

  }

  navigateBoard(id: number) {
    this.router.navigate(['board', id])
  }

  onDelete(boardId: number) {
    this.boardsService.deleteBoard(boardId).subscribe({
      next: (response) => {
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  EditBoard() {
    this.boardsService.editBoard(this.board.id, this.newBoardName).subscribe({
      next: (response) => {
        this.ShowEditForm = false;
      },
      error: (error) => {
        console.log(error);
      }
    })
  }
}
