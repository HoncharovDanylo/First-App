import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import { ListModel} from "../models/tasklist/list.model";
import {environment} from "../../environments/environment.development";
import {ListCreateModel} from "../models/tasklist/list-create.model";

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private http  : HttpClient) {}

  GetLists(boardId : number): Observable<ListModel[]> {
    return this.http.get<ListModel[]>(`${environment.apiUrl}/lists/by-board/${boardId}`);
  }
  CreateList(model : ListCreateModel): Observable<ListModel> {
     return this.http.post<ListModel>(`${environment.apiUrl}/lists/create`, model);
  }
  EditListName(model : ListCreateModel,id?:number): Observable<void> {
    return this.http.put<void>(`${environment.apiUrl}/lists/${id}`, model);
  }
  DeleteList(id?:number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/lists/${id}`);
  }

}
