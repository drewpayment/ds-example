import { UserInfo, UserType } from '@ds/core/shared';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


export function prepareUserInfo() {
  return function<T>(source: Observable<T>): Observable<T> {
    return new Observable(subscriber => {
      const subscription = source.pipe(
        map((piped) => {
          let user = piped as unknown as UserInfo;
          user.selectedClientId = () => {
            if (user.lastClientId !== null && user.lastClientId > 0)
              return user.lastClientId;
            return user.clientId;
          };
          user.isInMinimumRole = (type: UserType) => type >= user.userTypeId;
          user.isRole = (...types: UserType[]) => types.includes(user.userTypeId);
          user.lastClientId = user.lastClientId || user.clientId;

          user.selectedEmployeeId = () => {
            return user.lastEmployeeId || user.employeeId;
          };

          const isSupAndNotViewingThemselves =
            user.userTypeId === UserType.supervisor &&
            user.lastEmployeeId !== user.employeeId;
          const isValidRole =
            user.userTypeId === UserType.companyAdmin ||
            user.userTypeId === UserType.systemAdmin;

          user.userEmployeeId = user.employeeId;
          user.userFirstName = user.firstName;
          user.userLastName = user.lastName;

          if (isSupAndNotViewingThemselves || isValidRole) {
            if (user.userTypeId === UserType.companyAdmin) {
              user.$isEmulating = true;
              user.employeeId = user.lastEmployeeId;
              user.firstName = user.lastEmployeeFirstName;
              user.middleInitial = user.lastEmployeeMiddleInitial;
              user.lastName = user.lastEmployeeLastName;
            }
          }

          return user as unknown as T;
        }),
      ).subscribe({
        next(value) {
          subscriber.next(value);
        },
        error(error) {
          subscriber.error(error);
        },
        complete() {
          subscriber.complete();
        }
      });
      return () => subscription.unsubscribe();
    });
  }
}
