import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { UserInfo } from '@ds/core/shared';
import { Actions, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, mergeMap } from 'rxjs/operators';
import { LoadUser, UpdateUser, UserActionTypes } from './user.actions';

@Injectable()
export class UserEffects {
  // loadUser$ = this.actions$.pipe(
  //   ofType(UserActionTypes.LoadUser),
  //   mergeMap(() =>
  //     this.http.get<UserInfo>(`api/account/userinfo`).pipe(
  //       map((user) => new UpdateUser(user)),
  //       catchError((err) => of({ type: '[User] Failed to load User' }))
  //     )
  //   )
  // );

  constructor(
    private actions$: Actions,
    // private http: HttpClient
  ) {}
}
