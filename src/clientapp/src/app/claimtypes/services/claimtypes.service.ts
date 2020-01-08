import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Injectable } from '@angular/core';

import { HttpWrapperService } from '../../shared/services/index';

import {
  ClaimType
} from '../models/index';

import { ClaimtypesModule } from '../claimtypes.module';

@Injectable({
  providedIn: ClaimtypesModule
})
export class ClaimTypesService {
  public constructor(
    private http: HttpWrapperService
  ) { }

  public claimtypes(): Observable<Array<ClaimType>> {
    return this.http
      .get<Array<ClaimType>>('claimtype')
      .pipe(catchError(this.http.handleError));
  }

  public claimtype(id: string): Observable<ClaimType> {
    return this.http
      .get<ClaimType>(`claimtype/${id}`)
      .pipe(catchError(this.http.handleError));
  }

  public create(model: ClaimType): Observable<string> {
    return this.http
      .post<string>('claimtype', model)
      .pipe(catchError(this.http.handleError));
  }

  public update(model: ClaimType): Observable<any> {
    return this.http
      .put<ClaimType>('claimtype', model)
      .pipe(catchError(this.http.handleError));
  }

  public delete(id: string): Observable<any> {
    return this.http
      .delete<any>(`claimtype/${id}`)
      .pipe(catchError(this.http.handleError));
  }
}
