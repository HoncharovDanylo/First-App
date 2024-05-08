import {applicationConfig, argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";
import {of} from "rxjs";
import {ListModel} from "../models/tasklist/list.model";
import {CardModalComponent} from "./card-modal.component";
import {HistoryService} from "../services/history.service";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {HttpClient, HttpClientModule, HttpHandler} from "@angular/common/http";

const example_data = {
  cards:[]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    cards: example_data
  }
}
const meta : Meta<CardModalComponent> =  {
  title: 'Card Modal',
  component: CardModalComponent,
  decorators: [
    moduleMetadata({
      imports: [],
      providers: [provideMockStore(initialstate),{
        provide: HistoryService,
        useValue: {
          getHistoryByCardId: (id: number) => of([])
        }

      }],
    }),
  applicationConfig({
    providers: [HistoryService, DynamicDialogConfig, DynamicDialogRef, provideMockStore(initialstate), HttpClient]
  }),],
  tags : ['autodocs'],
  render : (args) => ({
    props : {
      ...args
    },
    template: `<p-dialog header="Edit Profile" [modal]="true" [(visible)]="visible" ><app-card-modal ${argsToTemplate(args)}></app-card-modal></p-dialog>`

  }),
}
export default meta;

type CardModalComponentStory = StoryObj<CardModalComponent>;
export const Primary : CardModalComponentStory = {
  args:{
    Card : {
      id: 1,
      title: 'SomeCard 1',
      description: 'Some Description',
      dueDate: new Date('2022-12-12'),
      priority: 'High',
      taskListId: 0,
      taskListName : 'SomeList'
    },
  }};
