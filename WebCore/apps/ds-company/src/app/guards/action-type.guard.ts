import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ActionTypeGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.accountService.getAllowedActions().pipe(
        map(perms => {
          let hasAllPerms = true;
          let neededActionTypes = this.getActionTypes(next);
          neededActionTypes.forEach(nat => {
            if (!hasAllPerms) return;
            hasAllPerms = perms.includes(nat);
          });

          return hasAllPerms || this.router.parseUrl("/error");
        })
      );
  }
  
  private getActionTypes(state: ActivatedRouteSnapshot): string[] {
    let routeData = state.data;

    if (routeData && routeData.actionTypes && routeData.actionTypes.length)
      return routeData.actionTypes as string[];
    else if (!routeData.parent) 
      return [];

    return this.getActionTypes(state.parent);
  }

}
