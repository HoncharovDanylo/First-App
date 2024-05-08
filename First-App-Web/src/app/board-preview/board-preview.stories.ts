import {argsToTemplate, Meta, moduleMetadata, StoryObj} from "@storybook/angular";
import {BoardPreviewComponent} from "./board-preview.component";
import {MockStoreConfig, provideMockStore} from "@ngrx/store/testing";

const example_data = {
  boards:[]

}
const initialstate : MockStoreConfig<any> = {
  initialState: {
    boards: example_data
  }
}
const meta : Meta<BoardPreviewComponent> =  {
  title: 'Board Preview',
  component: BoardPreviewComponent,
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
    template: `<app-board-preview ${argsToTemplate(args)}></app-board-preview>`

  }),
}
export default meta;

type BoardPreviewComponentStory = StoryObj<BoardPreviewComponent>;
export const Primary : BoardPreviewComponentStory = {
    args:{
      board : {
        id: 1,
        name: 'Board 1',
      },
    },
};
