import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  AbstractControl,
  AsyncValidatorFn,
  ValidationErrors,
} from '@angular/forms';
import { AccountService } from '@ds/core/account.service';
import { Observable, of, throwError } from 'rxjs';
import {
  catchError,
  delay,
  dematerialize,
  map,
  materialize,
  mergeMap,
} from 'rxjs/operators';

interface UsernameAvailableResult {
  isValid: boolean;
  validationMessages: any;
  value: boolean;
}

const FAKE_NETWORK_ERROR = new HttpErrorResponse({
  error: {
    errors: [
      {
        errorMessage: 'Must be 4-15 characters',
        text: 'Must be 4-15 characters',
      },
    ],
  },
  status: 400,
  statusText: 'Must be 4-15 characters',
});

@Injectable({
  providedIn: 'root',
})
export class UniqueUserService {
  constructor(private account: AccountService) {}

  validate(username: string): Observable<boolean> {
    return this.account.getUserInfo().pipe(
      // This is the agreed upon length requirements for passwords in our application
      // based on AUTH-172
      mergeMap((user) =>
        !!username && username.length > 3 && username.length < 16
          ? this.account.isUsernameAvailable(user.userId, username)
          : throwError(FAKE_NETWORK_ERROR).pipe(
              materialize(),
              delay(300),
              dematerialize()
            )
      ),
      catchError((err: HttpErrorResponse) => {
        console.log(err, 'error');
        if (err.error.errors && err.error.errors[0]) {
          const errorMsg = err.error.errors[0].text;
          if (errorMsg.toLowerCase().includes('4-15'))
            return of({isValid: true});
        }
        return of(false);
      }),
      map((res: UsernameAvailableResult) => res && res.isValid)
    );
  }
}

@Injectable({
  providedIn: 'root',
})
export class FormValidatorService {
  constructor(private service: UniqueUserService) {}

  /**
   * Checks if the username is available. Offers the ability to pass in a property key
   * in a sibling state that checks to see if userid is set and it should skip the username check
   * over http.
   *
   * @param skipIfExisting false
   * @param userIdKey userId
   * @returns AsyncValidatorFn
   */
  validate(
    skipIfExisting = false,
    userIdKey: string = 'userId'
  ): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (skipIfExisting && control.parent) {
        const existingUid = +control.parent.get(userIdKey).value;
        if (!!existingUid) {
          return of(null);
        }
      }
      return this.service
        .validate(control.value)
        .pipe(map((res) => (!!res ? null : { unique: true })));
    };
  }
}
