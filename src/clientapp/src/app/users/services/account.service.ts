import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Injectable } from '@angular/core';

import { HttpWrapperService, IdentityResult } from '../../shared/services/index';

import {
  AssignClaims,
  AssignRoles,
  ChangePassword,
  RegisterUser,
  User
} from '../models/index';

@Injectable()
export class AccountService {
  public constructor(
    private http: HttpWrapperService
  ) { }

  public users(): Observable<Array<User>> {
    return this.http
      .get<Array<User>>('account/users')
      .pipe(catchError(this.http.handleError));
  }

  public user(id: string): Observable<User> {
    return this.http
      .get<User>(`account/user/${id}`)
      .pipe(catchError(this.http.handleError));
  }

  public register(model: RegisterUser): Observable<IdentityResult> {
    return this.http
      .post<IdentityResult>('account/register', model)
      .pipe(catchError(this.http.handleError));
  }

  public changePassword(model: ChangePassword): Observable<IdentityResult> {
    return this.http
      .post<IdentityResult>('account/changepassword', model)
      .pipe(catchError(this.http.handleError));
  }

  public assignClaims(model: AssignClaims): Observable<IdentityResult> {
    return this.http
      .put<IdentityResult>('account/assignclaims', model)
      .pipe(catchError(this.http.handleError));
  }

  public assignRoles(model: AssignRoles): Observable<IdentityResult> {
    return this.http
      .put<IdentityResult>('account/assignroles', model)
      .pipe(catchError(this.http.handleError));
  }
}
