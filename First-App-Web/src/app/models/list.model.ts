import {Card} from "./card.model";

export interface List{
  id : number,
  name : string,
  cardsCount : number,
  boardId : number,
  cards : Card[]
}
