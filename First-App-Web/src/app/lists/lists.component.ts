import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ListService} from "../services/list.service";
import { ListModel} from "../models/tasklist/list.model";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {TasksListComponent} from "../tasks-list/tasks-list.component";
import {FormControl, FormGroup, FormsModule, Validators} from "@angular/forms";
import {ListCreateModel} from "../models/tasklist/list-create.model";
import {Observable} from "rxjs";
import {Store} from "@ngrx/store";
import {AppState} from "../../app.state";
import {selectListsByBoard} from "../store/tasklists/tasklists.selectors";
import {TaskListsActions} from "../store/tasklists/tasklists-actions-type";

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [
    NgForOf,
    NgIf,
    TasksListComponent,
    FormsModule,
    AsyncPipe
  ],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css'
})
export class ListsComponent implements OnInit {
  @Input()  boardId? : number;
  showCreateList : boolean = false;
  TasksList!: Observable<ListModel[] | undefined>
  CreateList : ListCreateModel;
  CreateForm? : FormGroup

  constructor( private store : Store<AppState>) {
    this.CreateList= {
      name: '',
      boardId: 0
    }
  }

  ngOnInit(): void {
    this.TasksList = this.store.select(selectListsByBoard(this.boardId!))
    this.CreateForm = new FormGroup({
      listname: new FormControl(this.CreateList.name,[
        Validators.required,
        Validators.maxLength(100)])
    });
    this.CreateList.boardId = this.boardId!;
  }
  OnSubmit(){
    this.store.dispatch(TaskListsActions.createTaskList(this.CreateList))
    this.OnCreateAction();
  }
  OnCreateAction(){
    this.showCreateList = false;
    this.CreateList.name = '';
  }
}

