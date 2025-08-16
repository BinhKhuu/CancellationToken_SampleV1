import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';
import { msalInterceptorConfig, msalInstance, msalGuardConfig } from '../msConfig/msConfig'

import { routes } from './app.routes';
import {MsalInterceptor, MsalModule} from '@azure/msal-angular';
import {BrowserModule} from '@angular/platform-browser';
import {HTTP_INTERCEPTORS, provideHttpClient, withInterceptors} from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(
      BrowserModule,
      MsalModule.forRoot(msalInstance, msalGuardConfig, msalInterceptorConfig)
    ),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    }
  ]
};
