import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { forkJoin, Observable, of, zip } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { map, switchMap } from 'rxjs/operators';
import { UserInfo, UserType } from '@ds/core/shared';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Injectable({
  providedIn: 'root'
})
export class LandingPageGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      
      /** 
      * Check to see user went through email recovery
      */
      this.accountService.SyncUserRecoveryEmail().subscribe(resp => { 
        // nice
      }, err => {
        console.log("Error syncing recovery email: ", err);
      });


      /****************************************************
      * If an applicant is trying to access the home page
      * redirect the applicant to ApplicantJobBoard.aspx
      *****************************************************/
      return forkJoin(this.accountService.getUserInfo(), this.accountService.getSiteUrls())
        .pipe(switchMap(([user, sites]: [UserInfo, any]) => {
          const userTypes = this.getUserTypes(next);
          
          if (user.userTypeId == UserType.applicant) {
            const payroll = sites.find(s => s.siteType === ConfigUrlType.Payroll);
            if (payroll && payroll.url) {
              window.location.href = payroll.url + 'ApplicantJobBoard.aspx';
            }
          } else if (user.userTypeId == UserType.employee 
              && user.employeeId != null 
              && user.employeeId > 0) {
                return this.accountService.checkIfEmployeeIsActiveInOnboarding(user.employeeId)
                  .pipe(map(isInOnboarding => {
                    if (isInOnboarding) {
                      const ess = sites.find(s => s.siteType === ConfigUrlType.Ess);
                      window.location.href = ess.url + 'Onboarding';
                      // return this.router.parseUrl(ess.url + 'Onboarding');
                    } else {
                      return this.defaultPermissionsCheck(userTypes, user);
                    }
                  }));
          } else {
            return of(this.defaultPermissionsCheck(userTypes, user));
          }
        }));
  }

  private getUserTypes(state: ActivatedRouteSnapshot): UserType[] {
    let routeData = state.data;

    if (routeData && routeData.userTypes && routeData.userTypes.length)
      return routeData.userTypes as UserType[];
    else if (!routeData.parent) 
      return [];

    return this.getUserTypes(state.parent);
  }

  private defaultPermissionsCheck(userTypes: UserType[], user: UserInfo) {
    let hasPermission = userTypes.includes(user.userTypeId);
    return hasPermission || this.router.parseUrl("/labor/schedule/group");
  }

  private onboardingCheck(userTypes: UserType[], user: UserInfo, sites: any) {
    return this.accountService.checkIfEmployeeIsActiveInOnboarding(user.employeeId)
      .pipe(map(isInOnboarding => {
        if (isInOnboarding) {
          const ess = sites.find(s => s.siteType === ConfigUrlType.Ess);
          return this.router.parseUrl(ess.url + 'Onboarding');
        } else {
          return this.defaultPermissionsCheck(userTypes, user);
        }
      }));
  }
}

