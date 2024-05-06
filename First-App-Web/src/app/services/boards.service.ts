import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {BoardModel} from "../models/board/board.model";
import {CreateBoardModel} from "../models/board/create-board.model";

@Injectable({
  providedIn: 'root'
})
export class BoardsService {

  constructor(public http : HttpClient) { }

  getBoards() : Observable<BoardModel[]>{
    return this.http.get<BoardModel[]>(`${environment.apiUrl}/boards`)
  }
  getBoardById(id : number) : Observable<BoardModel>{
    return this.http.get<BoardModel>(`${environment.apiUrl}/boards/${id}`)
  }
  deleteBoard(id : number) : Observable<void> {
   return this.http.delete<void>(`${environment.apiUrl}/boards/${id}`)
  }
  createBoard(board : CreateBoardModel) : Observable<BoardModel>{
    return this.http.post<BoardModel>(`${environment.apiUrl}/boards/create`, board)
  }

  editBoard(id: number, board : CreateBoardModel) {
    return this.http.put<void>(`${environment.apiUrl}/boards/update/${id}`, board)
  }
}
