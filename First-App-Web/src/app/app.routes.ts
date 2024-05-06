import { Routes } from '@angular/router';
import {BoardComponent} from "./board/board.component";
import {ViewBoardsComponent} from "./view-boards/view-boards.component";
import {AppComponent} from "./app.component";
import {BoardsResolver} from "./store/boards.resolver";
import {SpecificBoardResolver} from "./store/specific-board.resolver";

export const routes: Routes = [
  { path: '', component:ViewBoardsComponent, resolve:{
    boards: BoardsResolver
    }},
  { path: 'board/:id', component: BoardComponent, resolve : {
    lists : SpecificBoardResolver
    }}

];
