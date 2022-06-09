import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable, Observer } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { PERFORMANCE_ACTIONS } from '@ds/performance/shared/performance-actions';

@Injectable({
    providedIn: 'root'
})
export class FeatureGuard implements CanActivate {

    constructor(private accountService: AccountService, private router: Router) {}

    /**
     * This is the main method of our feature guard and lets us do some explicit checks before
     * allowing the user to route to the specified route.
     *
     */
    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> | Promise<boolean> | boolean {


        /**
         * If the user is trying to navigate to the performance page, we check to make sure they have the feature.
         * We get the user's allowed actions and make sure that they have access to make it to the performance route.
         * If they do not, we simply change the destination of the router to the goals page.
         *
         * We assume that if they're attempting to get to this link that they at least have one of the features enabled
         * because otherwise the links to get to the performance module are hidden on the ESS site.
         *
         * TODO: This is probably a better method to authorize components and routes based on "features" that the client
         * has enabled. This is the "angular" way to do it and would enable us to redirect users to marketing pages, etc or show them
         * particular messaging if they have tried to navigate to a url they aren't supposed to.
         */
        if (state.url.includes('/performance')) {

            return Observable.create((observer:Observer<boolean>) =>
                this.accountService.getAllowedActions().toPromise()
                    .then(permissions => {
                        let isPerformanceEnabled:boolean = false;
                        let isGoalTrackingEnabled:boolean = false;

                        permissions.forEach(p => {
                            if(p === PERFORMANCE_ACTIONS.Performance.ReadReview) {
                                isPerformanceEnabled = true;
                            } else if(p === PERFORMANCE_ACTIONS.GoalTracking.ReadGoals) {
                                isGoalTrackingEnabled = true;
                            }
                        });

                        if (!isPerformanceEnabled && isGoalTrackingEnabled) {
                            if (!state.url.includes('goals')) {
                                this.router.navigate(['/performance/goals'], {replaceUrl: true, skipLocationChange: true});
                                observer.next(false);
                                observer.complete();
                            } else {
                                observer.next(true);
                                observer.complete();
                            }
                        } else if (isPerformanceEnabled) {
                            observer.next(true);
                            observer.complete();
                        } else {
                            /** User does not have either performance or goal tracking features enabled... */
                            observer.next(false);
                            observer.complete();
                        }
                    })
            );

        }

        return true;
    }
}
