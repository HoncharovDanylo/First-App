import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {HistoryModel} from "../models/History.model";

@Injectable({
  providedIn: 'root'
})
export class HistoryService {

  constructor(private http : HttpClient) { }

  getHistoriesByBoard(page : number, boardId : number): Observable<HistoryModel[]>{
    return this.http.get<HistoryModel[]>(`${environment.apiUrl}/history/board/${boardId}/${page}`)
  }
  getHistoryByCardId(cardId : number): Observable<HistoryModel[]>{
    return this.http.get<HistoryModel[]>(`${environment.apiUrl}/history/card/${cardId}`)
  }
}
