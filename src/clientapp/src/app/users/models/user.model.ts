export interface User {
  id: string;
  userName: string;
  email: string;
  lockoutEnabled: boolean;
  claims: Array<string>;
  roles: Array<string>;
}
