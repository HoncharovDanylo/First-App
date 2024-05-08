import {Component, Input} from '@angular/core';
import {CardModel} from "../models/card/card.model";
import {AsyncPipe, DatePipe, NgForOf} from '@angular/common';
import {
  NgbDropdown,
  NgbDropdownButtonItem,
  NgbDropdownItem,
  NgbDropdownMenu,
  NgbDropdownToggle
} from "@ng-bootstrap/ng-bootstrap";
import {CardModalEditComponent} from "../card-modal-edit/card-modal-edit.component";
import {DialogService, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import {CardModalComponent} from "../card-modal/card-modal.component";
import {ListModel} from "../models/tasklist/list.model";
import {AppState} from "../../app.state";
import {Store} from "@ngrx/store";
import {CardsActions} from "../store/cards/cards-actions-type";

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
    AsyncPipe,
    DatePipe
  ],
  providers:[DialogService],
  templateUrl: './card.component.html',
  styleUrl: './card.component.css'
})
export class CardComponent {

  @Input() showModals: boolean = true;
  @Input() boardId!: number;
  @Input() Card!: CardModel;
  @Input() MovementsList! : Observable<ListModel[] | undefined>;
  editRef : DynamicDialogRef | undefined;
  infoRef : DynamicDialogRef | undefined;

 constructor(public dialogService : DialogService, private store : Store<AppState>) {}

 ChangeList(listId : number){
    this.store.dispatch(CardsActions.MoveCard({
      card : this.Card , listId :  listId}
    ));
 }
 onDelete(){
   this.store.dispatch(CardsActions.DeleteCard({cardId : this.Card?.id}));
 }
 openEditCardDialog(){
   if (this.showModals){
     this.editRef = this.dialogService.open(CardModalEditComponent,{
       data: {
         Card : this.Card,
         boardId : this.boardId,
       },
       header: 'Edit Card',
       styleClass : 'xl:w-6 lg:w-7 md:w-9 xs:max-w-screen, sm:max-w-screen dialog'
     })
   }

 }
 ShowCardModal(){
   if (this.showModals){
     this.infoRef = this.dialogService.open(CardModalComponent,{
       data: {
         Card : this.Card,
         boardId : this.boardId,
       },
       header: 'Card info',
       contentStyle: {padding:'0'},
       styleClass: 'card-modal xl:w-10 lg:w-10 md:w-10 xs:max-w-screen, sm:max-w-screen dialog'
     })
     this.infoRef.onClose.subscribe((res : boolean)=>{
       if (res)
         this.openEditCardDialog();
     });
   }

  }
}
