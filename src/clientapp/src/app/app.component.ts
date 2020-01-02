import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

import { UserService } from './shared';
import { ClientConfigProvider } from './core';

@Component({
  selector: 'tw-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit {
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
    private oauthService: OAuthService,
    private clientConfigProvider: ClientConfigProvider
  ) {
    console.log('initializing app.component...');
   }

  public ngOnInit(): void {
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
    const config = this.clientConfigProvider.config;

    this.oauthService.configure({
      clientId: config.clientId,
      issuer: config.issuer,
      redirectUri: config.redirectUri || window.location.origin,
      scope: config.scope,
      loginUrl: config.loginUrl,
      logoutUrl: config.logoutUrl
    });
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin({
      onTokenReceived: context => {
        this.user.setProperties(context.accessToken);
      }
    });
  }
}
