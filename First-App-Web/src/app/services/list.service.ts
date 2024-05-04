import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {List} from "../models/list.model";
import {environment} from "../../environments/environment.development";
import {ListCreateModel} from "../models/list-create.model";
import {CardlessListModel} from "../models/cardless-list.model";

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private http  : HttpClient) {}

  GetLists(boardId : number): Observable<List[]> {
    return this.http.get<List[]>(`${environment.apiUrl}/lists/by-board/${boardId}`);
  }
  GetList(id? : number): Observable<List> {
    return this.http.get<List>(`${environment.apiUrl}/lists/${id}`);
  }
  GetListsForMove(id:number): Observable<CardlessListModel[]> {
    return this.http.get<CardlessListModel[]>(`${environment.apiUrl}/list/movements/${id}`);
  }
  GetAllListsForMove(firstId:number): Observable<CardlessListModel[]> {
    return this.http.get<CardlessListModel[]>(`${environment.apiUrl}/list/movements/all/${firstId}`);
  }
  GetListsNames(): Observable<CardlessListModel[]> {
    return this.http.get<CardlessListModel[]>(`${environment.apiUrl}/list/movements`);
  }
  CreateList(model : ListCreateModel): Observable<void> {
     return this.http.post<void>(`${environment.apiUrl}/lists/create`, model);
  }
  EditListName(model : ListCreateModel,id?:number): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/lists/${id}`, model);
  }
  DeleteList(id?:number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/lists/${id}`);
  }

}
