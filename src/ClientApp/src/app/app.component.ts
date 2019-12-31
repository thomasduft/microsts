import { Component } from '@angular/core';

import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

import { authConfig } from './models';
import { AuthService } from './shared';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'ClientApp';

  public get username(): string {
    return this.authService.userName;
  }

  public constructor(
    private authService: AuthService,
    private oauthService: OAuthService
  ) {
    this.configure();
  }

  public logout(): void {
    this.authService.logout();
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
  }
}
