import {createAction, props} from "@ngrx/store";
import {ListModel} from "../../models/tasklist/list.model";
import {ListCreateModel} from "../../models/tasklist/list-create.model";
import {Update} from "@ngrx/entity";

export const getTaskListsByBoard = createAction(
  '[TaskLists Resolver] Get TaskLists By Board',
  props<{boardId : number}>()
)
export const taskListsLoaded = createAction(
  '[Get TaskLists Effect] TaskLists Loaded',
  props<{taskLists : ListModel[]}>()
)

export const createTaskList = createAction(
  '[Create TaskList Form] Create TaskList',
  props<{name : string, boardId : number}>()
)
export const taskListCreatedSuccess = createAction(
  '[Create TaskList Effect] TaskList Created Success',
  props<{taskList : ListModel}>()
)
export const taskListCreatedFailure = createAction(
  '[Create TaskList Effect] TaskList Created Failure',
  props<{error : any}>()
)

export const deleteTaskList = createAction(
  '[TaskList Component] Delete TaskList',
  props<{listId : number}>()
)
export const taskListDeletedSuccess = createAction(
  '[TaskList Effect] TaskList Deleted Success',
  props<{listId : number}>()
)
export const taskListDeletedFailure = createAction(
  '[TaskList Effect] TaskList Deleted Failure',
  props<{error : any}>()
)

export const updateTaskList = createAction(
  '[TaskList Component] Update TaskList',
  props<{list :ListCreateModel, listId : number }>()
)
export const taskListUpdatedSuccess = createAction(
  '[TaskList Effect] TaskList Updated Success',
  props<{update : Update<ListModel>}>()
)
export const taskListUpdatedFailure = createAction(
  '[TaskList Effect] TaskList Updated Failure',
  props<{error : any}>()
)

