import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AccountService } from '@ds/core/account.service';
import { switchMap, filter, map, tap } from 'rxjs/operators';
import { of, Observable, Subscription, BehaviorSubject } from 'rxjs';
import { SidebarConfiguration, SidebarMenuType } from './shared';

@Injectable({
    providedIn: 'root'
})
export class BootstrapService {

    private _bootstrapped = false;

    get isBootstrapped(): boolean {
        return this._bootstrapped;
    }
    set isBootstrapped(value: boolean) {
        this._bootstrapped = value;
    }

    private sidebarConfiguration = new BehaviorSubject<SidebarConfiguration>({
        hasPerformance: false,
        hasGoalTracking: false,
        sidebarType: SidebarMenuType.default
    });

    sidebarConfig$ = this.sidebarConfiguration.asObservable();

    constructor(private router: Router, private accountService: AccountService) {
        this.router.events
            .pipe(
                filter(event => event instanceof NavigationEnd),
                map((event: NavigationEnd) => event.url.includes('performance')),
                switchMap(isPerformanceLink =>
                    isPerformanceLink ? this.accountService.getAllowedActions() : of([] as string[])),
                map(permissions => {
                    let hasGoalTracking = false,
                        hasPerformance = false,
                        showPerformanceMenu = false;

                    if (!permissions.length) {
                        showPerformanceMenu = false;
                    } else {
                        hasGoalTracking = permissions.find(p => p.toLowerCase() === 'goaltracking.readgoals') != null;
                        hasPerformance = permissions.find(p => p.toLowerCase() === 'performance.readreview') != null;
                        showPerformanceMenu = true;
                    }

                    return {
                        hasPerformance,
                        hasGoalTracking,
                        sidebarType: showPerformanceMenu
                            ? SidebarMenuType.performance : SidebarMenuType.default
                    } as SidebarConfiguration;
                })
            ).subscribe(config => {
                this.sidebarConfiguration.next(config);
            });
    }
}
