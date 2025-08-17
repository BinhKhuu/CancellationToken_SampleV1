import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {BrowserModule} from '@angular/platform-browser';
import {MsalModule, MsalService} from '@azure/msal-angular';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MsalModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: []
})
export class AppComponent {
  title = 'CancellationToken';

  constructor(
    private msalService: MsalService,
  ) {
    this.msalService.handleRedirectObservable()
      .subscribe({
        next: data => {
          console.log('handled redirect',data);
          // this will automatically redirect to default route?
        }
      })
  }
}
