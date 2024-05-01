import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {environment} from "../../environments/environment.development";
import {CreateCardModel} from "../models/create-card.mode";
import { DatePipe } from '@angular/common';
import {Card} from "../models/card.model";


@Injectable({
  providedIn: 'root'
})
export class CardService {

  datepipe : DatePipe = new DatePipe('en-US');
  constructor(private http : HttpClient) { }

  MoveCard(cardId:number, listId:number): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/card/${cardId}/change-list/${listId}`,{});
  }
  CreateCard(card : CreateCardModel) : Observable<void> {
    return this.http.post<void>(`${environment.apiUrl}/cards/create`, {
      Title : card.Title,
      Description : card.Description,
      DueDate : this.datepipe.transform(card.DueDate, 'yyyy-MM-dd'),
      Priority : card.Priority,
      TaskListId : card.TaskListId
    });
  }
  DeleteCard(cardId : number) : Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/cards/delete/${cardId}`);
  }
  EditCard(id:number, card : CreateCardModel) : Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/cards/update/${id}`, card);
  }
  GetCard(id : number) : Observable<Card> {
    return this.http.get<Card>(`${environment.apiUrl}/cards/${id}`);
  }
}
