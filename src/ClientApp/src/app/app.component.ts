import { Component } from '@angular/core';

import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

import { authConfig } from './models';
import { UserService } from './shared';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'ClientApp';

  public get isAuthenticated(): boolean {
    return this.user.isAuthenticated;
  }

  public get username(): string {
    return this.user.userName;
  }

  public get claims(): Array<string> {
    return this.user.userClaims;
  }

  public constructor(
    private router: Router,
    private user: UserService,
    private oauthService: OAuthService
  ) {
    this.configure();
  }

  public login(): void {
    this.oauthService.initLoginFlow();
  }

  public logout(): void {
    this.oauthService.logOut(false);
    this.user.reset();
    this.router.navigate(['/']);
  }

  private configure() {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin({
      onTokenReceived: context => {
        this.user.setProperties(context.accessToken);
      }
    });
  }
}
