import {CreateCardModel} from "./create-card.mode";

export interface CardEditModel{
  CardId : number;
  Card : CreateCardModel;
  ListName : string;
}
