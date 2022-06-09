import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService, UserType } from '@ds/core/shared';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PayrollRequestGuard implements CanActivate {

constructor(
  private acctSvc: AccountService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    return this.acctSvc.getUserInfo().pipe(
      map(x => x.userTypeId <= UserType.companyAdmin));
  }

}
