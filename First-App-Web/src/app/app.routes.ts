import { Routes } from '@angular/router';
import {BoardComponent} from "./board/board.component";
import {ViewBoardsComponent} from "./view-boards/view-boards.component";
import {AppComponent} from "./app.component";

export const routes: Routes = [
  { path: '', component:ViewBoardsComponent},
  { path: 'board/:id', component: BoardComponent }

];
