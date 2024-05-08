import {Component, OnInit} from '@angular/core';
import {HistoryService} from "../services/history.service";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {Observable} from "rxjs";
import {CardModel} from '../models/card/card.model';
import {HistoryModel} from "../models/history/History.model";
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
visible : boolean = true;
  Card! : CardModel;
  CardHistory : Observable<HistoryModel[]> | undefined;
constructor(
            public config : DynamicDialogConfig ,
            public ref: DynamicDialogRef<CardModalComponent>,public historyService : HistoryService,) {}

  ngOnInit(): void {
    this.Card = this.config!.data.Card;
    this.CardHistory = this.historyService!.getHistoryByCardId(this.config!.data.Card.id);
    }
    onEdit(){
      this.ref!.close(true)
    }
}
