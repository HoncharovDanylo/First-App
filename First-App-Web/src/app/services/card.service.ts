import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment.development";
import {CreateCardModel} from "../models/card/create-card.model";
import { DatePipe } from '@angular/common';
import {CardModel} from "../models/card/card.model";
import {Card} from "primeng/card";


@Injectable({
  providedIn: 'root'
})
export class CardService {

  datepipe : DatePipe = new DatePipe('en-US');
  constructor(private http : HttpClient) { }

  MoveCard(cardId:number, listId:number): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/card/${cardId}/change-list/${listId}`,{});
  }
  CreateCard(card : CreateCardModel) : Observable<CardModel> {

    return this.http.post<CardModel>(`${environment.apiUrl}/cards/create`, {
      Title : card.Title,
      Description : card.Description,
      DueDate : this.datepipe.transform(card.DueDate, 'yyyy-MM-dd', 'ua-UA'),
      Priority : card.Priority,
      TaskListId : card.TaskListId
    });
  }
  DeleteCard(cardId : number) : Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/cards/delete/${cardId}`);
  }
  EditCard(id:number, card : CreateCardModel) : Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/cards/update/${id}`, {
      Title : card.Title,
      Description : card.Description,
      DueDate : this.datepipe.transform(card.DueDate, 'yyyy-MM-dd', 'ua-UA'),
      Priority : card.Priority,
      TaskListId : card.TaskListId
    });
  }
  GetCardsForBoard(boardId : number) : Observable<CardModel[]> {
    return this.http.get<CardModel[]>(`${environment.apiUrl}/cards/by-board/${boardId}`);
  }
}
