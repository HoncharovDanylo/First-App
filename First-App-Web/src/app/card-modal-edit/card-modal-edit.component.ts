import {Component, OnInit} from '@angular/core';
import {FormsModule} from "@angular/forms";
import {MatOption} from "@angular/material/autocomplete";
import {MatFormField, MatLabel, MatSelect} from "@angular/material/select";
import {MatDatepicker, MatDatepickerInput, MatDatepickerToggle} from "@angular/material/datepicker";
import {MatInput} from "@angular/material/input";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {InputTextModule} from "primeng/inputtext";
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";
import {InputTextareaModule} from "primeng/inputtextarea";
import {Observable} from "rxjs";
import {AsyncPipe, NgIf} from "@angular/common";
import {MessageModule} from "primeng/message";
import {CreateCardModel} from "../models/card/create-card.model";
import {AppState} from "../../app.state";
import {Store} from "@ngrx/store";
import {GetListsForMoveWithCurrent} from "../store/tasklists/tasklists.selectors";
import {CardsActions} from "../store/cards/cards-actions-type";
import {CardModel} from "../models/card/card.model";
import {Update} from "@ngrx/entity";
import {ListModel} from "../models/tasklist/list.model";

@Component({
  selector: 'app-card-modal-edit',
  standalone: true,
  imports: [
    FormsModule,
    MatOption,
    MatSelect,
    MatLabel,
    MatFormField,
    MatDatepickerToggle,
    MatDatepicker,
    MatDatepickerInput,
    MatInput,
    InputTextModule,
    DropdownModule,
    CalendarModule,
    InputTextareaModule,
    AsyncPipe,
    MessageModule,
    NgIf
  ],
  templateUrl: './card-modal-edit.component.html',
  styleUrl: './card-modal-edit.component.css'
})
export class CardModalEditComponent implements OnInit{
  MovementsList : Observable<ListModel[]> | undefined;
  CardEdit : CreateCardModel={
    Title : '',
    Description : '',
    DueDate : new Date(),
    Priority : '',
    TaskListId : 0
  };
  priority : string ='';
  today = new Date();
  constructor(public dialogRef : DynamicDialogRef<CardModalEditComponent>,
              public dialogConfig : DynamicDialogConfig, private store : Store<AppState>) {
  }

  ngOnInit(): void {
      let card = this.dialogConfig.data.Card
      this.CardEdit.Title = card.title;
      this.CardEdit.Description = card.description;
      this.CardEdit.DueDate = new Date(card.dueDate);
      this.CardEdit.Priority = card.priority;
      this.CardEdit.TaskListId = card.taskListId;
      this.MovementsList =  this.store.select(GetListsForMoveWithCurrent(this.dialogConfig.data.boardId, this.dialogConfig.data.listId));


  }
  OnFormSubmit(){
    this.store.dispatch(CardsActions.UpdateCard({update : this.CardEdit, id : this.dialogConfig.data.Card.id}))
    this.dialogRef.close();
  }
  closeDialog(){
    this.dialogRef.close()
  }
}
