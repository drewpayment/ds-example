import {
    Component, QueryList, AfterViewInit,
    ContentChildren, OnInit, OnDestroy
} from '@angular/core';
import { UserInfo, IApplicationLink } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { Router, NavigationEnd } from '@angular/router';
import { filter, map, switchMap, tap } from 'rxjs/operators';
import { Observable, of, Subscription } from 'rxjs';
import { BootstrapService } from '../bootstrap.service';
import { SidebarMenuType } from '../shared';

@Component({
    selector: 'ess-sidebar',
    templateUrl: './sidebar.component.html',
    styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit, OnDestroy {

    @ContentChildren('[routerLink]') menu: QueryList<any>;

    user: UserInfo;
    hasPerformance = false;
    hasGoalTracking = false;
    showPerformanceMenu$: Observable<boolean>;
    showPerformanceMenu = false;
    subs: Subscription[] = [];

    constructor(private bs: BootstrapService) { }

    ngOnInit() {
        this.subs.push(this.bs.sidebarConfig$.subscribe(config => {
            this.hasPerformance = config.hasPerformance;
            this.hasGoalTracking = config.hasGoalTracking;
            this.showPerformanceMenu = config.sidebarType === SidebarMenuType.performance;
        }));
    }

    ngOnDestroy() {
        this.subs.forEach(s => s.unsubscribe());
    }

    setActive() {
        // console.dir(event);
    }

}
