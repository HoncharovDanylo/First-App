import {argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";
import {CardComponent} from "./card.component";
import {of} from "rxjs";
import {ListModel} from "../models/tasklist/list.model";

const example_data = {
  cards:[]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    cards: example_data
  }
}
const meta : Meta<CardComponent> =  {
  title: 'Card',
  component: CardComponent,
  decorators: [
    moduleMetadata({
      imports: [],
      providers: [provideMockStore(initialstate)]
    })],
  tags : ['autodocs'],
  render : (args) => ({
    props : {
      ...args
    },
    template: `<app-card ${argsToTemplate(args)}></app-card>`

  }),
}
export default meta;

type CardComponentStory = StoryObj<CardComponent>;
export const Primary : CardComponentStory = {
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
    boardId : 1,
    showModals : false,
    MovementsList : of([
      {id: 1, name: 'List 1'},
      {id: 2, name: 'List 2'},
      {id: 3, name: 'List 3'}] as ListModel[])
}};
