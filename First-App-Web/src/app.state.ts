import {BoardModel} from "./app/models/board/board.model";
import { ListModel} from "./app/models/tasklist/list.model";
import {CardModel} from "./app/models/card/card.model";

export interface AppState{
  readonly boards : BoardModel[];
  readonly tasklists : ListModel[];
  readonly cards : CardModel[];
}
