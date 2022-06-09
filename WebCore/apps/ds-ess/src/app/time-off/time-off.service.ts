import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { tap, map } from 'rxjs/operators';
import { TimeOffPolicy } from '../shared';
import { AccountService } from '@ds/core/account.service';


@Injectable({
    providedIn: 'root'
})
export class TimeOffService {
    static canRequestTimeOffAction = 'LeaveManagement.TimeOffRequest';
    private policies$ = new BehaviorSubject<TimeOffPolicy[]>(null);
    get policies() {
        return this.policies$.asObservable();
    }

    private _selectedPolicy$ = new BehaviorSubject<TimeOffPolicy>(null);
    get selectedPolicy$(): Observable<TimeOffPolicy> {
        return this._selectedPolicy$.asObservable();
    }

    private _hasMultiplePolicies = new BehaviorSubject<boolean>(false);
    get hasMultiplePolicies(): Observable<boolean> {
        return this._hasMultiplePolicies.asObservable();
    }

    private _canRequestTimeOff = new BehaviorSubject<boolean>(false);
    get canRequestTimeOff(): boolean {
        return this._canRequestTimeOff.getValue();
    }
    set canRequestTimeOff(value: boolean) {
        this._canRequestTimeOff.next(value);
    }

    constructor(private http: HttpClient, private account: AccountService) {}

    getTimeOffPolicyActivity(employeeId: number): Observable<TimeOffPolicy[]> {
        const url = `api/employee/${employeeId}/timeoff/active`;
        return this.http.get<TimeOffPolicy[]>(url)
            .pipe(
                tap(policies => {
                    this.policies$.next(policies);
                    this._hasMultiplePolicies.next(policies && policies.length > 1);
                })
            );
    }

    loadPoliciesAndSetByName(employeeId: number, name: string): void {
        this.getTimeOffPolicyActivity(employeeId)
            .subscribe(policies => {
                this.setPolicyByName(name);
            });
    }

    setPolicyByName(name: string): void {
        const policies = this.policies$.getValue();
        if (!policies || !policies.length) {
            return;
        }

        const found = policies.find(p => p.policyName.trim().toLowerCase() === name.trim().toLowerCase());
        this._selectedPolicy$.next(found);
    }

    setPolicy(p: TimeOffPolicy) {
        this._selectedPolicy$.next(p);
    }

    getRequestTimeOffPermission(): Observable<boolean> {
        return this.account.canPerformActions(TimeOffService.canRequestTimeOffAction)
            .pipe(map(canDo => {
                if (typeof canDo === 'boolean') {
                    this.canRequestTimeOff = canDo;
                } else {
                    this.canRequestTimeOff = false;
                }
                return this.canRequestTimeOff;
            }));
    }
}
