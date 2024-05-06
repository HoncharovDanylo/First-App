import {Component, Input, OnInit} from '@angular/core';
import {HistoryModel} from "../models/history/History.model";
import {HistoryService} from "../services/history.service";
import {ButtonModule} from "primeng/button";
import {DatePipe, NgForOf, NgIf} from "@angular/common";


@Component({
  selector: 'app-history-general',
  standalone: true,
  imports: [
    ButtonModule,
    NgForOf,
    DatePipe,
    NgIf,
  ],
  templateUrl: './history-general.component.html',
  styleUrl: './history-general.component.css'
})
export class HistoryGeneralComponent implements OnInit{
  @Input() BoardId : number = 0;
page : number = 1;
histories : HistoryModel[] =[];
ShowMore : boolean = true;
constructor(public historyService : HistoryService) {}

  ngOnInit(): void {
    this.GetHistories();
  }
  GetHistories(){
    this.historyService.getHistoriesByBoard(this.page++, this.BoardId).subscribe({
      next: (data) => {
        this.histories = this.histories.concat(data);
        this.ShowMore = data.length == 20;
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
