export interface HistoryModel{
  cardName : string;
  listName : string;
  action : string;
  field : string;
  previousValue? : string;
  newValue? : string;
  timestamp : Date;
}
