import {Component, OnInit} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {MatFormField, MatOption, MatSelect} from "@angular/material/select";
import {MatDatepicker, MatDatepickerToggle, MatDatepickerModule} from "@angular/material/datepicker";
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';
import {CreateCardModel} from "../models/card/create-card.model";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import { InputTextModule } from 'primeng/inputtext'
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";
import {InputTextareaModule} from "primeng/inputtextarea";
import {Observable} from "rxjs";
import {Store} from "@ngrx/store";
import {AppState} from "../../app.state";
import {CardsActions} from "../store/cards/cards-actions-type";
import {ListModel} from "../models/tasklist/list.model";
import {GetListsForMoveWithCurrent} from "../store/tasklists/tasklists.selectors";
@Component({
  selector: 'app-card-modal-create',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    InputTextModule,
    DropdownModule,
    CalendarModule,
    InputTextareaModule,
    AsyncPipe,
    ReactiveFormsModule,
    NgIf,

  ],
  providers : [provideNativeDateAdapter()],
  templateUrl: './card-modal-create.component.html',
  styleUrl: './card-modal-create.component.css'
})

export class CardModalCreateComponent implements OnInit{
MovementsList : Observable<ListModel[]> | undefined;
SelectedValue : CreateCardModel={
  Title : '',
  Description : '',
  DueDate : new Date(),
  Priority : 'Low',
  TaskListId : 0
};
today = new Date();
priority : string ='';
  constructor(public dialogRef : DynamicDialogRef<CardModalCreateComponent>, public dialogConfig : DynamicDialogConfig, private store : Store<AppState>) {

  }

  ngOnInit(): void {
    this.MovementsList = this.store.select(GetListsForMoveWithCurrent(this.dialogConfig.data.boardId, this.dialogConfig.data.listId));
    this.SelectedValue.TaskListId = this.dialogConfig.data.listId;
    }
OnFormSubmit(){
  this.store.dispatch(CardsActions.CreateCard({card : this.SelectedValue}));
  this.closeDialog()
}
closeDialog(){
    this.dialogRef.close()
}
}
