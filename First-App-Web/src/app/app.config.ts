import {ApplicationConfig, importProvidersFrom} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HttpClientModule} from "@angular/common/http";
import {MatDialogModule} from "@angular/material/dialog";
import {provideAnimations} from "@angular/platform-browser/animations";
import {MatNativeDateModule} from "@angular/material/core";

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    importProvidersFrom(HttpClientModule),
    importProvidersFrom(MatDialogModule),
    provideAnimations(),
    importProvidersFrom(MatNativeDateModule)
  ],
};
