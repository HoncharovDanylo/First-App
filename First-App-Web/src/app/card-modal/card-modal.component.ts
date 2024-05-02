import {Component, OnInit} from '@angular/core';
import {HistoryService} from "../services/history.service";
import {CardService} from "../services/card.service";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import { Card } from '../models/card.model';
import {HistoryModel} from "../models/History.model";
import {ButtonModule} from "primeng/button";
import {AsyncPipe, DatePipe, NgForOf, NgIf} from "@angular/common";
import {CalendarModule} from "primeng/calendar";
import {DropdownModule} from "primeng/dropdown";
import {InputTextareaModule} from "primeng/inputtextarea";
import {PaginatorModule} from "primeng/paginator";

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
            public ref: DynamicDialogRef<CardModalComponent>) {}

  ngOnInit(): void {
    this.Card = this.cardService.GetCard(this.config.data.CardId);
    this.CardHistory = this.historyService.getHistoryByCardId(this.config.data.CardId);
    }
    onEdit(){
      this.ref.close(true)
    }
}
