import {argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";
import {of} from "rxjs";
import {ListModel} from "../models/tasklist/list.model";
import {ListsComponent} from "./lists.component";

const example_data = {
  ids : [1,2,3],
  entities : [
    {
      id: 1,
      name: 'List 1',
      boardId: 1
    },
    {
      id: 2,
      name: 'List 2',
      boardId: 1
    },
    {
      id: 3,
      name: 'List 3',
      boardId: 1
    }

  ]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    cards: example_data
  }
}
const meta : Meta<ListsComponent> =  {
  title: 'Lists',
  component: ListsComponent,
  decorators: [
    moduleMetadata({
      imports: [],
      providers: [provideMockStore(initialstate)],
    })],
  tags : ['autodocs'],
  render : (args) => ({
    props : {
      ...args
    },
    template: `<app-lists ${argsToTemplate(args)}></app-lists>`

  }),
}
export default meta;

type ListComponentStory = StoryObj<ListsComponent>;
export const Primary : ListComponentStory = {
  args: {
    boardId : 1
  }
}
