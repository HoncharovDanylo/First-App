import {ApplicationConfig, importProvidersFrom, isDevMode} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HttpClientModule} from "@angular/common/http";
import {MatDialogModule} from "@angular/material/dialog";
import {provideAnimations} from "@angular/platform-browser/animations";
import {MatNativeDateModule} from "@angular/material/core";
import { provideStore } from '@ngrx/store';
import { provideStoreDevtools } from '@ngrx/store-devtools';
import {BoardsResolver} from "./store/boards.resolver";
import { provideEffects } from '@ngrx/effects';
import {BoardsEffects} from "./store/boards/boards.effects";
import {boardsReducer} from "./store/boards/boards.reducers";
import {ListsEffects} from "./store/tasklists/tasklists.effects";
import {SpecificBoardResolver} from "./store/specific-board.resolver";
import {listsReducer} from "./store/tasklists/tasklists.reducers";
import {CardsEffects} from "./store/cards/cards.effects";
import {cardsReducer} from "./store/cards/cards.reducers";

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    importProvidersFrom(HttpClientModule),
    importProvidersFrom(MatDialogModule),
    provideAnimations(),
    importProvidersFrom(MatNativeDateModule),
    provideStore({boards : boardsReducer , lists : listsReducer, cards : cardsReducer}),
    provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() }),
    BoardsResolver,
    SpecificBoardResolver,
    provideEffects(BoardsEffects, ListsEffects, CardsEffects)

],
};
