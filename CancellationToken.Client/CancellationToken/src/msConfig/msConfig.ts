// src/app/msal-config.ts
import {BrowserCacheLocation, Configuration, InteractionType, PublicClientApplication} from '@azure/msal-browser';
import {MsalGuardConfiguration, MsalInterceptorConfiguration} from '@azure/msal-angular';

export const msalInstance = new PublicClientApplication({
  auth: {
    clientId: '1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d',
    authority: 'https://login.microsoftonline.com/fb2492a7-4cfe-4506-95f9-2284aa380e41',
    redirectUri: 'http://localhost:4200/auth-redirect',
    postLogoutRedirectUri: 'http://localhost:4200'
  },
  cache: {
    cacheLocation: BrowserCacheLocation.SessionStorage,
    storeAuthStateInCookie: false,
  },
  system: {
    loggerOptions: {
      loggerCallback: () => {},
      piiLoggingEnabled: false,
    },
  },
});

export const msalInterceptorConfig: MsalInterceptorConfiguration = {
  interactionType: InteractionType.Redirect,
  protectedResourceMap: new Map([])
};

export const msalGuardConfig: MsalGuardConfiguration = {
  interactionType: InteractionType.Redirect,
};
