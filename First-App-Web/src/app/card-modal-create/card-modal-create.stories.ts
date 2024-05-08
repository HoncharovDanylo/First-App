import {applicationConfig, argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";
import {of} from "rxjs";
import {ListModel} from "../models/tasklist/list.model";
import {HistoryService} from "../services/history.service";
import {DynamicDialogConfig, DynamicDialogRef} from "primeng/dynamicdialog";
import {HttpClient, HttpClientModule, HttpHandler} from "@angular/common/http";
import {CardModalCreateComponent} from "./card-modal-create.component";
import {DropdownModule} from "primeng/dropdown";
import {CalendarModule} from "primeng/calendar";

const example_data = {
  cards:[]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    cards: example_data
  }
}
const meta : Meta<CardModalCreateComponent> =  {
  title: 'Card Modal Create',
  component: CardModalCreateComponent,
  decorators: [
    moduleMetadata({
      imports: [DropdownModule, CalendarModule, HttpClientModule],
      providers: [provideMockStore(initialstate),{
        provide: HistoryService,
        useValue: {
          getHistoryByCardId: (id: number) => of([])
        },

      }, DropdownModule, CalendarModule],
    }),
    applicationConfig({
      providers: [ DynamicDialogConfig, DynamicDialogRef, provideMockStore(initialstate), HttpClient]
    }),],
  tags : ['autodocs'],
  render : (args) => ({
    props : {
      ...args
    },
    template: `<app-card-modal-create ${argsToTemplate(args)}></app-card-modal-create>`

  }),
}
export default meta;

type CardModalCreateComponentStory = StoryObj<CardModalCreateComponent>;
export const Primary : CardModalCreateComponentStory = {
  args:{

  }};
