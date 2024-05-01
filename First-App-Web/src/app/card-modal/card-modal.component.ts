import {Component, OnInit} from '@angular/core';
import {HistoryService} from "../services/history.service";
import {CardService} from "../services/card.service";
import {DialogService, DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import { Card } from '../models/card.model';
import {HistoryModel} from "../models/History.model";
import {ButtonModule} from "primeng/button";
import {AsyncPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {CalendarModule} from "primeng/calendar";
import {DropdownModule} from "primeng/dropdown";
import {InputTextareaModule} from "primeng/inputtextarea";
import {PaginatorModule} from "primeng/paginator";
import {CardModalEditComponent} from "../card-modal-edit/card-modal-edit.component";

@Component({
  selector: 'app-card-modal',
  standalone: true,
  imports: [
    ButtonModule,
    DatePipe,
    NgForOf,
    NgIf,
    AsyncPipe,
    CalendarModule,
    DropdownModule,
    InputTextareaModule,
    PaginatorModule
  ],
  templateUrl: './card-modal.component.html',
  styleUrl: './card-modal.component.css'
})
export class CardModalComponent implements OnInit{

  Card : Observable<Card> | undefined;
  CardHistory : Observable<HistoryModel[]> | undefined;
constructor(public historyService : HistoryService,
            public cardService : CardService,
            public config : DynamicDialogConfig ,
            public ref: DynamicDialogRef<CardModalComponent>,
            public dialogService : DialogService) {
}

  ngOnInit(): void {
    this.Card = this.cardService.GetCard(this.config.data.CardId);
    this.CardHistory = this.historyService.getHistoryByCardId(this.config.data.CardId);
    console.log(this.config.data.CardId)
    this.CardHistory.subscribe({
      next : value => console.log(value),
    })
    }
    onEdit(){
      this.dialogService.open(CardModalEditComponent, {
        data: {
          CardId: this.config.data.CardId
        },
        header: 'Edit Card',
        width: '40vw'
      });
      this.ref.close()
    }
}
