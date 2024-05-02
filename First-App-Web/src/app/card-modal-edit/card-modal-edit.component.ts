import {Component, OnInit} from '@angular/core';
import {CardlessListModel} from "../models/cardless-list.model";
import {ListService} from "../services/list.service";
import {CardService} from "../services/card.service";
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
import {CreateCardModel} from "../models/create-card.mode";

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
  MovementsList : Observable<CardlessListModel[]> | undefined;
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
              public dialogConfig : DynamicDialogConfig,
              private cardService : CardService, private listservice : ListService) {
  }

  ngOnInit(): void {
   this.cardService.GetCard(this.dialogConfig.data.CardId).subscribe({
    next: (value) => {
      let date = new Date(value.dueDate);
      this.CardEdit = {
        Title : value.title,
        Description : value.description,
        DueDate : date,
        Priority : value.priority,
        TaskListId : value.taskListId
      }
      this.MovementsList =  this.listservice.GetAllListsForMove(value.taskListId);
    },
    error: (error) => {
      console.log(error);
    }

  });

  }
  OnFormSubmit(){
    this.cardService.EditCard(this.dialogConfig.data.CardId,this.CardEdit!).subscribe({
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
