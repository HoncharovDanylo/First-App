import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {ListService} from "../services/list.service";
import {List} from "../models/list.model";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {CardComponent} from "../card/card.component";
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {CardlessListModel} from "../models/cardless-list.model";
import {CardModalCreateComponent} from "../card-modal-create/card-modal-create.component";
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {MatFormFieldModule} from "@angular/material/form-field";
import {DialogService, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
@Component({
  selector: 'app-tasks-list',
  standalone: true,
  imports: [
    NgForOf,
    CardComponent,
    NgbDropdownModule,
    FormsModule,
    ReactiveFormsModule,
    NgIf,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    AsyncPipe
  ],
  providers : [DialogService],
  templateUrl: './tasks-list.component.html',
  styleUrl: './tasks-list.component.css'
})
export class TasksListComponent implements OnInit{

  @Input() TaskList: List = {
    id: 0,
    name: '',
    cardsCount: 0,
    cards: [],
    boardId: 0
  };
  AvailableList : Observable<CardlessListModel[]> | undefined;
  ShowListEdit : boolean = false;
  NewListName : string = this.TaskList.name;
  editForm? : FormGroup;
  ref: DynamicDialogRef | undefined;
  @Output() listUpdateEvent = new EventEmitter<void>();
constructor(private listservice : ListService, public dialogService : DialogService) {

}

  ngOnInit(): void {
    this.AvailableList = this.listservice.GetListsForMove(this.TaskList.id);
    this.editForm = new FormGroup({
      listname: new FormControl(this.NewListName, [
        Validators.required,
        Validators.maxLength(100)
      ]),
    });

    }
  OnEditSubmit(){

    this.ShowListEdit = false;
    this.listservice.EditListName( {name : this.NewListName, boardId : this.TaskList.boardId},this.TaskList.id).subscribe({
      next: () => {
        this.TaskList.name = this.NewListName;
        this.NewListName = this.TaskList.name;
      },
      error: (error) => {
        console.log(error);
      }
    });

  }
  onDelete(){
    this.listservice.DeleteList(this.TaskList.id).subscribe({
      next: () => {
        this.listUpdateEvent.emit();
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
  openCreateCardDialog(){
    this.ref = this.dialogService.open(CardModalCreateComponent, {
      data :{id: this.TaskList.id, name: this.TaskList.name},
      header: 'Create Card',
      styleClass: 'xl:w-6 lg:w-7 md:w-9 xs:max-w-screen, sm:max-w-screen dialog',
    });
    this.ref.onClose.subscribe(() => {
      this.listUpdateEvent.emit();
    });
  }

}
