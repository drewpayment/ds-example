import { Component, OnInit, NgZone, Inject } from '@angular/core';
import { TimeOffPolicy, TimeOffHistoricalReport, TimeOffEvent } from '../../shared';
import { TimeOffService } from '../time-off.service';
import { Observable, of, Subscription } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { ActivatedRoute, ParamMap, Router, NavigationEnd } from '@angular/router';
import * as moment from 'moment';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { MessageTypes } from '@ajs/core/msg/ds-msg-msgTypes.enumeration';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { TimeOffStatusType } from '@ds/core/employee-services/models';
import { FormGroup, FormControl } from '@angular/forms';


@Component({
    selector: 'ds-time-off-activity',
    templateUrl: './time-off-activity.component.html',
    styleUrls: ['./time-off-activity.component.scss']
})
export class TimeOffActivityComponent implements OnInit {
    user: UserInfo;
    params: ParamMap;
    policy$: Observable<TimeOffPolicy>;
    policies$: Observable<TimeOffPolicy[]>;
    policies: TimeOffPolicy[];
    hasMultiplePolicies$: Observable<boolean>;
    canRequestTimeOff = false;
    selectedPolicy$: any;
    get historicalReports(): TimeOffHistoricalReport[] {
        return [
            { reportTypeId: 1, name: 'Request History' },
            { reportTypeId: 2, name: 'Benefit History' }
        ];
    }

    refreshPageSub: Subscription;
    eventTypes = {
        request: { name: 'Request' },
        balance: { name: 'Balance' },
        award: { name: 'Award' },
        adjustment: { name: 'Adjustment' },
        expiration: { name: 'Expiration' }
    };

    eventStatusTypes = [
        { eventStatusTypeId: 1, name: 'Pending' },
        { eventStatusTypeId: 2, name: 'Approved' },
        { eventStatusTypeId: 3, name: 'Rejected' },
        { eventStatusTypeId: 4, name: 'Cancelled' },
    ];
    activePolicy: TimeOffPolicy;

    constructor(
        private service: TimeOffService,
        private route: ActivatedRoute,
        private msg: DsMsgService,
        private dsPopup: DsPopupService,
        private account: AccountService,
        private zone: NgZone,
        private router: Router,
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
        this.selectedPolicy$ = this.service.selectedPolicy$;
        this.canRequestTimeOff = this.service.canRequestTimeOff;
        this.policies$ = this.service.policies.pipe(tap(p => this.policies = p));
        this.hasMultiplePolicies$ = this.service.hasMultiplePolicies.pipe(tap(b => {
            return b
        }));
        this.policy$ = this.route.paramMap
            .pipe(
                switchMap((params: ParamMap) => {
                    this.params = params;
                    return this.account.getUserInfo();
                }),
                switchMap(u => {
                    this.user = u;
                    return this.service.getRequestTimeOffPermission();
                }),
                switchMap(canDo => {
                    this.canRequestTimeOff = canDo;
                    return this.service.selectedPolicy$;
                }),
                switchMap(policy => {
                    if (!policy) {
                        this.service.loadPoliciesAndSetByName(this.user.employeeId, this.params.get('policyName'));
                    }
                    return of(policy);
                }),
                tap(policy => {
                    if (!policy) {
                        this.service.loadPoliciesAndSetByName(this.user.employeeId, this.params.get('policyName'));
                    } else {
                        this.activePolicy = policy;
                    }
                })
            );
    }

    changeSelectedPolicy(event) {
        const name = event.target.value.trim().toLowerCase();
        const selectedPolicy = this.policies.find(p => p.policyName.trim().toLowerCase() === name);
        if(selectedPolicy) {
            this.service.setPolicyByName(selectedPolicy.policyName);
            this.router.navigate(['/timeoff', selectedPolicy.policyName, 'activity']);
        }

    }

    getReportUrl(policy: TimeOffPolicy, report: TimeOffHistoricalReport) {
        const from = moment(policy.startingUnitsAsOf).add(-1, 'days').add(-1, 'years');
        const to = moment(policy.startingUnitsAsOf).add(-1, 'days');

        return 'Legacy/API/GetLeaveManagementEmployeeReport.ashx' +
            '?PolicyId='+ policy.policyId +
            '&FromDate=' + from.format('MM/DD/YYYY') +
            '&ToDate=' + to.format('MM/DD/YYYY') +
            '&ReportTypeId=' + report.reportTypeId;
    }

    goToReport(id: number) {
        const report = this.historicalReports.find(h => h.reportTypeId === id);
        return this.getReportUrl(this.activePolicy, report);
    }

    editTimeOffEvent(event: TimeOffEvent) {
        var popupUrl = 'Legacy/RequestTimeOffPopup.aspx?';

        if(this.service.canRequestTimeOff) {
            if(event) {
                popupUrl += 'strRequestTimeOffID=' + event.requestTimeOffId;
            }
            else {
                popupUrl += 'strRequestTimeOffID=0';
            }

            popupUrl += '&strEmployeeID=' + this.user.employeeId + "&FromESS=1";

            this.openPopup(popupUrl);
        } else {
            this.msg.setTemporaryMessage('You do not have permission to request time off.', MessageTypes.error, 3000);
        }
    }

    handleClose(retVal) {
        const url = this.document.location.href;
        this.router.navigateByUrl(url);
    }

    friendlyDuration(event: TimeOffEvent) {
        const start = moment(event.startDate);
        const end = moment(event.endDate);
        if (start.isSame(end))
            return start.format("MMMM D, YYYY");
        if (start.isSame(end, 'month'))
            return start.format("MMMM D\u2009\u2013\u2009") + end.format("D, YYYY");
        return start.format("MMMM D\u2009\u2013\u2009") + end.format("MMMM D, YYYY");
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

        const left = (xt - x) / 2;
        const top = (yt - y) / 4;

        const modal = this.dsPopup.open(url, '_blank', { height: y, width: x, top: top, left: left });

        modal.closed().then(res => {
            this.service.loadPoliciesAndSetByName(this.user.employeeId, this.params.get('policyName'));
        })
    }

    isRequestEvent(event: TimeOffEvent): boolean {
        return event && event.requestTimeOffId != null && event.requestTimeOffId > 0;
    }

    isAwardEvent(event: TimeOffEvent): boolean {
        return event && event.timeOffAward && event.timeOffAward > 0;
    }

    isPositiveAward(event: TimeOffEvent) {
        return this.isAwardEvent(event) && event.amount > 0;
    }

    getEventType(event: TimeOffEvent) {
        if (this.isAwardEvent(event)) {
            if (event.timeOffAward === 1) {
                return this.eventTypes.award;
            }
            if (event.timeOffAward === 4) {
                return this.eventTypes.adjustment;
            }
            return this.eventTypes.expiration;
        }
        if (this.isRequestEvent(event)) {
            return this.eventTypes.request;
        }
        return this.eventTypes.balance;
    }

    getEventStatusType(event: TimeOffEvent) {
        if (this.isAwardEvent(event))
            return { eventStatusTypeId: null, name: 'Projected' };
        else
            return this.eventStatusTypes.find(e => e.eventStatusTypeId == event.timeOffStatus);
    }

    isPending(event: TimeOffEvent) {
        return this.isRequestEvent(event) && event.timeOffStatus === TimeOffStatusType.Pending; // PENDING
    }

    isApproved(event: TimeOffEvent) {
        return this.isRequestEvent(event) && event.timeOffStatus === TimeOffStatusType.Approved; // APPROVED
    }

    isRejected(event: TimeOffEvent) {
        return this.isRequestEvent(event) && event.timeOffStatus === TimeOffStatusType.Rejected; // REJECTED
    }
    isCancelled(event: TimeOffEvent) {
        return this.isRequestEvent(event) && event.timeOffStatus === TimeOffStatusType.Cancelled; // REJECTED
    }

    isNegativeAward(event: TimeOffEvent) {
        return this.isAwardEvent(event) && event.amount < 0;
    }

    isProjected(e: TimeOffEvent) {
        return this.isAwardEvent(e);
    }

    requestNew() {
        let popupUrl = 'Legacy/RequestTimeOffPopup.aspx?';

        if (this.canRequestTimeOff) {
            popupUrl += 'strRequestTimeOffID=0&strEmployeeID=' + this.user.employeeId + '&ClientAccrualID=' + this.activePolicy.policyId + "&FromESS=1";

            this.openPopup(popupUrl);
        } else {
            this.msg.setTemporaryMessage('You do not have permission to request time off.', MessageTypes.error, 3500);
        }
    }

}
