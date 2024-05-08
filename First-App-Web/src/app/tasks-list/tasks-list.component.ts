import {Component, Input, OnInit} from '@angular/core';
import { ListModel} from "../models/tasklist/list.model";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {CardComponent} from "../card/card.component";
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import {FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {CardModalCreateComponent} from "../card-modal-create/card-modal-create.component";
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {MatFormFieldModule} from "@angular/material/form-field";
import {DialogService, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import {Store} from "@ngrx/store";
import {AppState} from "../../app.state";
import {TaskListsActions} from "../store/tasklists/tasklists-actions-type";
import {CardModel} from "../models/card/card.model";
import {GetCountOfCardsByList, selectCardsByList} from "../store/cards/cards.selectors";
import {GetListsForMove} from "../store/tasklists/tasklists.selectors";
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
  @Input() ShowModals: boolean = true;
  @Input() TaskList: ListModel = {
    id: 0,
    name: '',
    boardId: 0
  };
  AvailableList! : Observable<ListModel[] | undefined>;
  Cards! : Observable<CardModel[] | undefined>;
  ShowListEdit : boolean = false;
  NewListName : string = this.TaskList.name;
  CardsCount! : Observable<number | undefined>
  editForm? : FormGroup;
  ref: DynamicDialogRef | undefined;
constructor( private store : Store<AppState>,public dialogService : DialogService) {

}

  ngOnInit(): void {
    this.Cards = this.store.select(selectCardsByList(this.TaskList.id))
    this.CardsCount = this.store.select(GetCountOfCardsByList(this.TaskList.id))
    this.AvailableList = this.store.select(GetListsForMove(this.TaskList.boardId, this.TaskList.id));

  }
  OnEditSubmit(){
    this.ShowListEdit = false;
    this.store.dispatch(TaskListsActions.updateTaskList({list : {name : this.NewListName, boardId : this.TaskList.boardId}, listId : this.TaskList.id}));
  }
  onDelete(){
    this.store.dispatch(TaskListsActions.deleteTaskList({listId : this.TaskList.id}));
  }
  openCreateCardDialog(){
  if(this.ShowModals){
    this.ref = this.dialogService.open(CardModalCreateComponent, {
      data :{listId: this.TaskList.id, name: this.TaskList.name, boardId: this.TaskList.boardId},
      header: 'Create Card',
      styleClass: 'xl:w-6 lg:w-7 md:w-9 xs:max-w-screen, sm:max-w-screen dialog',
    });
  }

  }

}
