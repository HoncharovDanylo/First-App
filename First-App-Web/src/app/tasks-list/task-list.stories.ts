import {argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";
import {of} from "rxjs";
import {ListModel} from "../models/tasklist/list.model";
import {TasksListComponent} from "./tasks-list.component";

const example_data = {
  cards:[]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    cards: example_data
  }
}
const meta : Meta<TasksListComponent> =  {
  title: 'TaskList',
  component: TasksListComponent,
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
    template: `<div style="width:270px">
              <app-tasks-list ${argsToTemplate(args)}></app-tasks-list>

                </div>`

  }),
}
export default meta;

type TaskListComponentStory = StoryObj<TasksListComponent>;
export const Primary : TaskListComponentStory = {
  args:{
    TaskList:{
      id: 1,
      name: 'SomeList',
      boardId: 1
    },
    ShowModals : false

  }};
