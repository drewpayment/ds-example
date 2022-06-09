import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { map } from 'rxjs/operators';
import { UserType } from '@ds/core/shared';

@Injectable({
  providedIn: 'root'
})
export class UserTypeGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.accountService.getUserInfo()
        .pipe(
          map(user => {
            const userTypes = this.getUserTypes(next);
            let hasPermission = userTypes.includes(user.userTypeId);

            return hasPermission || this.router.parseUrl("/error");
          }
        )
      );
  }

  private getUserTypes(state: ActivatedRouteSnapshot): UserType[] {
    let routeData = state.data;

    if (routeData && routeData.userTypes && routeData.userTypes.length)
      return routeData.userTypes as UserType[];
    else if (!routeData.parent) 
      return [];

    return this.getUserTypes(state.parent);
  }
}

