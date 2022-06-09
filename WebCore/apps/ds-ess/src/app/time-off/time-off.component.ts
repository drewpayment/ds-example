import { Component, OnInit, Directive, ElementRef, Injector, Inject } from '@angular/core';
import { UpgradeComponent } from '@angular/upgrade/static';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { TimeOffPolicy, TimeOffUnit } from '../shared';
import { TimeOffService } from './time-off.service';
import { Observable, of, Subscription, BehaviorSubject, combineLatest } from 'rxjs';
import { tap, switchMap, catchError, map } from 'rxjs/operators';
import { Router, NavigationEnd } from '@angular/router';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { DOCUMENT } from '@angular/common';

export const ScopeProviderFactory = (injector: Injector) => injector.get('$rootScope').$new();

export const ScopeProvider = {
    deps: ['$injector'],
    provide: '$scope',
    useFactory: ScopeProviderFactory
};

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: 'ajs-timeoff',
})
// tslint:disable-next-line: directive-class-suffix
export class TimeOffSummaryDirective extends UpgradeComponent {
    constructor(elemRef: ElementRef, injector: Injector) {
        super('ajsTimeoffComponent', elemRef, injector);
    }
}

@Component({
    selector: 'ds-timeoff-summary',
    templateUrl: './time-off.component.html',
    styleUrls: ['./time-off.component.scss']
})
export class TimeOffComponent implements OnInit {
    user: UserInfo;
    policies$: Observable<TimeOffPolicy[]>;
    canRequestTimeOff = true;
    isLoading = true;
    refreshPageSub: Subscription;
    refreshSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    constructor(
        private service: TimeOffService,
        private account: AccountService,
        private router: Router,
        private msg: DsMsgService,
        private dsPopup: DsPopupService,
        @Inject(DOCUMENT) private document: Document
    ) {
        this.router.routeReuseStrategy.shouldReuseRoute = function() {
            return false;
        };
        this.refreshPageSub = this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.router.navigated = false;
            }
        });
    }

    ngOnInit() {
        (window as any).handleClose = (val) => this.handleClose(val);
        this.policies$ = combineLatest(this.account.getUserInfo(), this.refreshSubject.asObservable())
            .pipe(
                tap(res => this.user = res[0]),
                switchMap(_ => this.account.canPerformActions(TimeOffService.canRequestTimeOffAction)),
                catchError(_ => {
                    this.canRequestTimeOff = false;
                    return of(_);
                }),
                tap(_ => this.service.canRequestTimeOff = this.canRequestTimeOff),
                switchMap(_ => this.service.getTimeOffPolicyActivity(this.user.employeeId)),
                tap(_ => this.isLoading = false)
            );
    }

    requestNew() {
        let popupUrl = 'Legacy/RequestTimeOffPopup.aspx?';

        if (this.canRequestTimeOff) {
            popupUrl += 'strRequestTimeOffID=0&strEmployeeID=' + this.user.employeeId + '&FromESS=1';

            this.openPopup(popupUrl);
        } else {
            this.msg.setTemporaryMessage('You do not have permission to request time off.', MessageTypes.error, 3500);
        }
    }

    handleClose(retVal) {
        const url = this.document.location.href;
        this.router.navigateByUrl(url);
    }

    viewActivity(policy: TimeOffPolicy) {
        this.service.setPolicy(policy);
        this.router.navigate(['timeoff', policy.policyName, 'activity']);
    }

    openPopup(url: String) {
        const w = window,
            d = document,
            e = d.documentElement,
            g = d.getElementsByTagName('body')[0],
            x = 820,
            y = 624,
            xt = w.innerWidth || e.clientWidth || g.clientWidth,
            yt = w.innerHeight || e.clientHeight || g.clientHeight;

        const modal = this.dsPopup.open(url, '', { height: y, width: x });

        modal.closed().then(res => {
            this.refreshSubject.next(null);
        })
    }

}
