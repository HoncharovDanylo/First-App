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
  priority : string ='';
  today = new Date();
  constructor(public dialogRef : DynamicDialogRef<CardModalEditComponent>,
              public dialogConfig : DynamicDialogConfig,
              private cardService : CardService, private listservice : ListService) {
  }

  ngOnInit(): void {
   this.MovementsList =  this.listservice.GetAllListsForMove(this.dialogConfig.data.Card.TaskListId);
  }
  OnFormSubmit(){
    console.log(this.dialogConfig.data.Card.DueDate)
    this.cardService.EditCard(this.dialogConfig.data.CardId,this.dialogConfig.data.Card).subscribe({
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
