import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { take, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
/**********************************************************
 * @author Collin Larson
 * @version 1.0
 * @description Company Admin Guard is the security guard / bouncer
 * for pages that require company admin user rights.
 *********************************************************/
export class CompanyAdminGuard implements CanActivate {

    /*********************************************************
   * Creates an instance of company admin guard.
   * @param accountService AccountService
   * @description Constructs the CompanyAdminGuard with its needed
   * dependicies
   ********************************************************/
  constructor(private accountService: AccountService) { }

  /***************************************************************************
   * Determines whether a user can access a page
   * @param next ActivatedRouteSnapshot (angular routing, next component)
   * @param state ActivatedRouteSnapshot (angular routing, current component)
   * @returns activate
   **************************************************************************/
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.accountService.getUserInfo().pipe(
        take(1),
        map(user => user.userTypeId == 2 )
      );
  }
  
}
