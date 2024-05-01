import {Component, OnDestroy, OnInit} from '@angular/core';
import {ListService} from "../services/list.service";
import {List} from "../models/list.model";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {TasksListComponent} from "../tasks-list/tasks-list.component";
import {FormControl, FormGroup, FormsModule, Validators} from "@angular/forms";
import {ListCreateModel} from "../models/list-create.model";
import {Observable} from "rxjs";

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
  showCreateList : boolean = false;
  TasksList: Observable<List[]> | undefined
  CreateList : ListCreateModel;
  CreateForm? : FormGroup

  constructor(private listsService: ListService) {
    this.CreateList= {
      name: ''
    }
  }

  ngOnInit(): void {
    this.TasksList = this.listsService.GetLists();
    this.CreateForm = new FormGroup({
      listname: new FormControl(this.CreateList.name,[
        Validators.required,
        Validators.maxLength(100)])
    });
  }
  OnSubmit(){
    console.log(this.CreateList)
    this.listsService.CreateList(this.CreateList).subscribe({
      next: (response) => {
        this.OnCreateAction();
        this.ngOnInit();
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
  OnCreateAction(){
    this.showCreateList = false;
    this.CreateList.name = '';
  }
}

