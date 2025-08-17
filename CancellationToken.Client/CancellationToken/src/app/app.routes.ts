import { Routes } from '@angular/router';
import {MsalGuard, MsalRedirectComponent} from '@azure/msal-angular';
import { AppComponent } from './app.component';

export const routes: Routes = [
  {
    path: 'auth-redirect',
    component: MsalRedirectComponent
  },
  {
    path: '',
    canActivate: [MsalGuard],
    component: AppComponent
  }
];
