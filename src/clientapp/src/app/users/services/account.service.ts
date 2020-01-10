import { Observable, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { Injectable } from '@angular/core';

import { HttpWrapperService, IdentityResult } from '../../shared/services/index';

import { ClaimTypesService } from '../../claimtypes/services';
import { RoleService } from '../../roles/services';

import {
  AssignClaims,
  AssignRoles,
  ChangePassword,
  RegisterUser,
  User,
  UserDetail
} from '../models/index';

@Injectable()
export class AccountService {
  public constructor(
    private http: HttpWrapperService,
    private claimsService: ClaimTypesService,
    private roleService: RoleService
  ) { }

  public users(): Observable<Array<User>> {
    return this.http
      .get<Array<User>>('account/users')
      .pipe(catchError(this.http.handleError));
  }

  public user(id: string): Observable<UserDetail> {
    return forkJoin({
      user: this.http.get<User>(`account/user/${id}`),
      claims: this.claimsService.claimtypes(),
      roles: this.roleService.roles()
    })
      .pipe(map(info => {
        return {
          user: info.user,
          claims: info.claims.map(c => c.name),
          roles: info.roles.map(r => r.name)
        };
      }))
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

  public update(model: User): Observable<IdentityResult> {
    return this.http
      .put<IdentityResult>('account/user', model)
      .pipe(catchError(this.http.handleError));
  }

  public delete(id: string): Observable<any> {
    return this.http
      .delete<any>(`account/user/${id}`)
      .pipe(catchError(this.http.handleError));
  }

  // public assignClaims(model: AssignClaims): Observable<IdentityResult> {
  //   return this.http
  //     .put<IdentityResult>('account/assignclaims', model)
  //     .pipe(catchError(this.http.handleError));
  // }

  // public assignRoles(model: AssignRoles): Observable<IdentityResult> {
  //   return this.http
  //     .put<IdentityResult>('account/assignroles', model)
  //     .pipe(catchError(this.http.handleError));
  // }
}
