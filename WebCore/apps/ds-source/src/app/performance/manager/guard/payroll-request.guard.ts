import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, zip, combineLatest } from 'rxjs';
import { UserType, UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { map, concatMap, share, tap } from 'rxjs/operators';
import { IReviewProfileBasicSetup } from '@ds/performance/review-profiles/shared/review-profile-basic-setup.model';
import { Maybe } from '@ds/core/shared/Maybe';
import { ReviewPolicyApiService } from '@ds/performance/review-policy/review-policy-api.service';
import { PerformanceManagerService } from '@ds/performance/performance-manager/performance-manager.service';
import { IReviewTemplate } from '@ds/core/groups/shared/review-template.model';

@Injectable({
  providedIn: 'root'
})
export class PayrollRequestGuard implements CanActivate {

  constructor(
    private reviewPolicySvc:ReviewPolicyApiService,
    private manager:PerformanceManagerService,
    private acctSvc: AccountService,
    public router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

        return CanUserViewPayrollRequests(
          this.acctSvc.getUserInfo,
          this.reviewPolicySvc.getReviewTemplatesByClientId,
          this.reviewPolicySvc.getReviewProfileSetups,
          this.manager.selectedReviewTemplate$,
          (x) => {
            if(x !== true){
              this.router.navigate(['/performance/manage']);
            }
          },
          false,
          null,
          null,
          null);
  }

}
/**
 * This takes a lot of parameters.  Maybe create a generic partial
 * apply pipe that returns a function which calls the provided
 * function with its parameters applied?  Doing that would cause
 * this function to be impure.....  I'm not sure what the best
 * way is to fix this problem
 *
 * If any of these functions need a certain 'this' then make sure that
 * they are defined as typescript lambda functions
 */
export function CanUserViewPayrollRequests(
  getUserInfo: (reloadUser?: boolean, disableEmulation?: boolean) => Observable<UserInfo>,
  getReviewTemplates: (clientId: number, isArchived: boolean, reload: boolean) => Observable<IReviewTemplate[]>,
  getReviewProfileSetups: (id: number[]) => Observable<IReviewProfileBasicSetup[]>,
  selectedReviewTemplate$: Observable<IReviewTemplate>,
  handleResult: (result: boolean) => void,
  isArchived?: boolean,
  reload?: boolean,
  reloadUser?: boolean,
  disableEmulation?: boolean
) {

  const userHasPermissiontoSeePayrollRequestsTab$ = getUserInfo(reloadUser, disableEmulation).pipe(
    map(x => x.userTypeId <= UserType.companyAdmin));

  const getReviewTemplates$ = getUserInfo(reloadUser, disableEmulation).pipe(concatMap(x => {
    return getReviewTemplates(x.clientId, isArchived, reload).pipe(share());
  }))

  const getReviewProfiles$ = getReviewTemplates$.pipe(concatMap(cycles => {
    const profileIds = cycles.map(x => x.reviewProfileId).reduce((x, y) => x.concat(y), []);
    const existing: { [id: number]: boolean } = {};
    const result: number[] = [];
    profileIds.forEach(x => {
      if (existing[x] == null) {
        result.push(x);
      }

      existing[x] = true;
    });
    const profIds: { [id: number]: IReviewProfileBasicSetup } = {};
    return getReviewProfileSetups(result).pipe(
      map(x => {
        (x || []).forEach(y => profIds[y.reviewProfileId] = y);
        return profIds;
      }));
  }));

  return combineLatest(getReviewProfiles$,
    userHasPermissiontoSeePayrollRequestsTab$,
    selectedReviewTemplate$).pipe(
    map(x => ({ reviewProfs: x[0], hasPermission: x[1], review: x[2] })),
    map(x =>
      new Maybe(x.reviewProfs)
      .map(reviewProfs => reviewProfs[new Maybe(x.review).map(x => x.reviewProfileId).value()])
      .map(y => y.includePayrollRequests).value() === true && x.hasPermission === true),
    tap(handleResult));
}


