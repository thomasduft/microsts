import { Injectable, EventEmitter } from '@angular/core';

import { OAuthService } from 'angular-oauth2-oidc';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private static ANONYMOUS = 'anonymous';

  private username: string = AuthService.ANONYMOUS;
  private claims: Array<string> = new Array<string>();

  public onAuthenticated: EventEmitter<boolean> = new EventEmitter<boolean>();

  public get isAuthenticated(): boolean {
    return this.userName !== AuthService.ANONYMOUS;
  }

  public get userName(): string {
    const claims = this.oauthService.getIdentityClaims();

    if (claims) {
      // tslint:disable-next-line: no-string-literal
      return claims['name'];
    }

    return AuthService.ANONYMOUS;
  }

  public get userClaims(): Array<string> {
    return this.claims;
  }

  public constructor(
    private oauthService: OAuthService
  ) { }

  public getToken(): string {
    return this.oauthService.getAccessToken();
  }

  public login(): void {
    this.oauthService.initImplicitFlow();

    this.setProperties();

    this.onAuthenticated.next(this.isAuthenticated);
  }

  public logout(): void {
    this.oauthService.logOut(true);

    this.username = AuthService.ANONYMOUS;

    this.onAuthenticated.next(this.isAuthenticated);
  }

  public hasClaim(claim: string): boolean {
    if (!this.claims || !claim) {
      return false;
    }

    return this.claims.some((r => r === claim));
  }

  private setProperties(): void {
    const accesToken = this.oauthService.getAccessToken();
    if (accesToken) {
      const jwt = JSON.parse(window.atob(accesToken.split('.')[1]));

      this.username = jwt.name;

      this.claims = Array.isArray(jwt.role)
        ? jwt.role
        : [jwt.role];

      return;
    }

    this.username = AuthService.ANONYMOUS;
    this.claims = new Array<string>();
  }
}
