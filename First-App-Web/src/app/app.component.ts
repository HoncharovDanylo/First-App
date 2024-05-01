import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {TasksListComponent} from "./tasks-list/tasks-list.component";
import {ListsComponent} from "./lists/lists.component";
import {MatDialog} from "@angular/material/dialog";
import { SidebarModule } from 'primeng/sidebar';
import {HistoryGeneralComponent} from "./history-general/history-general.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TasksListComponent, ListsComponent, SidebarModule, HistoryGeneralComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'First-App-Web';
  sidebarVisible = false
  constructor(public dialog: MatDialog) {
  }

  historyClick(){
    this.sidebarVisible = true
  }
}
