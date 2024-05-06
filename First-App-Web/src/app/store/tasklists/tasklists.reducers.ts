import {createEntityAdapter, EntityState} from "@ngrx/entity";
import {ListModel} from "../../models/tasklist/list.model";
import {createReducer, on} from "@ngrx/store";
import {TaskListsActions} from "./tasklists-actions-type";

export interface ListsState extends EntityState<ListModel>{}

export const adapter = createEntityAdapter<ListModel>();
export const  initialBoardsState = adapter.getInitialState();

export const listsReducer = createReducer(
  initialBoardsState,
  on(TaskListsActions.getTaskListsByBoard,
    (state, action) => {return state}),
  on(TaskListsActions.taskListsLoaded,
    (state, action) =>
      adapter.addMany(action.taskLists, state)),

  on(TaskListsActions.createTaskList,
    (state, action) => {return state}),
  on(TaskListsActions.taskListCreatedSuccess,
    (state, action) =>
      adapter.addOne(action.taskList, state)),
  on(TaskListsActions.taskListCreatedFailure,
    (state, action) => {
      console.log(action.error);
      return state
    }),

  on(TaskListsActions.deleteTaskList,
    (state, action) => {return state}),
  on(TaskListsActions.taskListDeletedSuccess,
    (state,action) => adapter.removeOne(action.listId, state)),
  on(TaskListsActions.taskListDeletedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  }),

  on(TaskListsActions.updateTaskList,
    (state, action) => {return state}),
  on(TaskListsActions.taskListUpdatedSuccess,
    (state, action) => adapter.updateOne(action.update, state)),
  on(TaskListsActions.taskListUpdatedFailure,
    (state, action) => {
    console.log(action.error);
    return state
  })
)
export const {selectAll} = adapter.getSelectors();
