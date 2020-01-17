export interface User {
  id: string;
  userName: string;
  email: string;
  lockoutEnabled: boolean;
  isLockedOut: boolean;
  claims: Array<string>;
  roles: Array<string>;
}
