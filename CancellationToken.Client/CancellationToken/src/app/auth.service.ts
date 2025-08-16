import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private msalService: MsalService
  ) { }

  login(){
    this.msalService.loginRedirect();
  }

  logout(){
    this.msalService.logoutRedirect();
  }

  silentLogin(){
    return this.msalService.acquireTokenSilent(
    {
      scopes: ['User.Read']
    });
  }

}
