import { Injectable } from '@angular/core';
import * as http from "node:http";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {List} from "./models/list.model";

@Injectable({
  providedIn: 'root'
})
export class ListService {

  constructor(private http  : HttpClient) {}

  GetLists(): Observable<List[]> {
    return this.http.get<List[]>('http://localhost:8080/api/lists');
  }
}
