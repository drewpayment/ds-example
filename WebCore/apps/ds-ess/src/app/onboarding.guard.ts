import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Injectable, Inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { OnboardingService } from './onboarding/onboarding.service';
import { AccountService } from '@ds/core/account.service';
import { map, switchMap, tap } from 'rxjs/operators';
import { DOCUMENT, APP_BASE_HREF } from '@angular/common';
import { joinWithSlash } from '@ds/core/app-config/app-config';
import { UserInfo } from '@ds/core/shared';
import { FinalizeCompletionStatusCheck } from './shared';


@Injectable({
    providedIn: 'root'
})
export class OnboardingGuard implements CanActivate {

    get window(): any {
        return this.document.defaultView;
    }

    private _user: UserInfo;

    constructor(
        private accountService: AccountService,
        private router: Router,
        @Inject(DOCUMENT) private document: Document,
        @Inject(APP_BASE_HREF) private baseHref: string,
        private onboardingService: OnboardingService
    ) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.accountService.getUserInfo().pipe(
            tap(u => this._user = u),
            switchMap(() =>
                this._user.isInOnboarding ? this.onboardingService.getFinalizeStatus(this._user.employeeId) : of(null)),
            map((result: FinalizeCompletionStatusCheck) => {
                if (result && !result.isFinalizeComplete) {
                    this.window.location = joinWithSlash(this.baseHref, 'onboarding');
                    return false;
                }
                return true;
            }),
        );
    }

}
