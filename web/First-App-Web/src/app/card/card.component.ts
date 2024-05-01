import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Card} from "../models/card.model";
import {AsyncPipe, DatePipe, NgForOf} from '@angular/common';
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {ListService} from "../services/list.service";
import {CardlessListModel} from "../models/cardless-list.model";
import {CardService} from "../services/card.service";
import {MatDialog} from "@angular/material/dialog";
import {CardModalEditComponent} from "../card-modal-edit/card-modal-edit.component";
import {DialogService, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import {CardModalComponent} from "../card-modal/card-modal.component";
import {Element} from "@angular/compiler";

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [
    NgbDropdown,
    NgbDropdownButtonItem,
    NgbDropdownItem,
    NgbDropdownMenu,
    NgbDropdownToggle,
    NgForOf,
    AsyncPipe
  ],
  providers:[DialogService],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {
 @Input() Card?: Card;
 @Input() MovementsList : Observable<CardlessListModel[]> | undefined;
 @Output() cardEdited = new EventEmitter<void>();
 editRef : DynamicDialogRef | undefined;
 infoRef : DynamicDialogRef | undefined;

 constructor(private cardService : CardService,public dialogService : DialogService) {
   console.log(this.MovementsList)
 }

 ChangeList(listId : number){
   this.cardService.MoveCard(this.Card?.id || 0,listId).subscribe({
     next: () => {
       this.cardEdited.emit();

     },
     error: (error) => {
       console.log(error);
     }
   });
 }
 onDelete(){
  this.cardService.DeleteCard(this.Card?.id || 0).subscribe({
      next: () => {
        this.cardEdited.emit();
      },
      error: (error) => {
        console.log(error);
      }
    });
 }
 openEditCardDialog(){
   this.editRef = this.dialogService.open(CardModalEditComponent,{
      data: {
        Card : {
          Title : this.Card?.title,
          Description : this.Card?.description,
          DueDate : this.Card?.dueDate,
          Priority : this.Card?.priority,
          TaskListId : this.Card?.taskListId
        },
        CardId : this.Card?.id,
        ListName : this.Card?.taskListName
      },
      width: '40vw',
      header: 'Edit Card',

   })
   this.editRef.onClose.subscribe(Result=>{
     this.cardEdited.emit();
   });
 }
 ShowCardModal(){
    this.infoRef = this.dialogService.open(CardModalComponent,{
      data: {
        CardId : this.Card?.id,
      },
      width: '80vw',
      header: 'Card info',
      contentStyle: {padding:'0'},
      styleClass: 'card-modal'
    })
 }
}
