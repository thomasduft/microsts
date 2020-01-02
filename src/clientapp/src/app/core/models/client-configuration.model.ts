export interface ClientConfiguration {
  clientId: string;
  issuer: string;
  redirectUri: string;
  scope: string;
  loginUrl: string;
  logoutUrl: string;
}
