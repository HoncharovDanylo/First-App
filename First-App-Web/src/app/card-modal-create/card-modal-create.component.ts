import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";
import {AsyncPipe, NgForOf, NgIf} from "@angular/common";
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {CardlessListModel} from "../models/cardless-list.model";
import {ListService} from "../services/list.service";
import {MatFormField, MatOption, MatSelect} from "@angular/material/select";
import {MatDatepicker, MatDatepickerToggle, MatDatepickerModule} from "@angular/material/datepicker";
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {provideNativeDateAdapter} from '@angular/material/core';
import {CreateCardModel} from "../models/create-card.mode";
import {CardService} from "../services/card.service";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import { InputTextModule } from 'primeng/inputtext'
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";
import {InputTextareaModule} from "primeng/inputtextarea";
import {Observable} from "rxjs";
@Component({
  selector: 'app-card-modal-create',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle,
    MatSelect,
    MatOption,
    MatFormField,
    MatDatepickerToggle,
    MatDatepicker,
    MatDatepickerModule,
    MatInputModule,
    MatFormFieldModule,
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
MovementsList : Observable<CardlessListModel[]> | undefined;
SelectedValue : CreateCardModel={
  Title : '',
  Description : '',
  DueDate : new Date(),
  Priority : 'Low',
  TaskListId : 0
};
today = new Date();
priority : string ='';
  constructor(public dialogRef : DynamicDialogRef<CardModalCreateComponent>, public dialogConfig : DynamicDialogConfig,
              private listservice : ListService,
              private cardService : CardService) {

  }

  ngOnInit(): void {
    this.MovementsList = this.listservice.GetAllListsForMove(this.dialogConfig.data.id);
    this.SelectedValue.TaskListId = this.dialogConfig.data.id;
    }
OnFormSubmit(){
  this.cardService.CreateCard(this.SelectedValue).subscribe({
    next: ()=>{
      this.closeDialog()
    },
    error: (error)=> {
      console.log(error);
    }
  });
}
closeDialog(){
    this.dialogRef.close()
}
}
