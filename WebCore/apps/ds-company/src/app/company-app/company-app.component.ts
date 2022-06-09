import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { DOCUMENT } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription, Subject } from 'rxjs';
import { map, takeUntil, tap } from 'rxjs/operators';
import { Store } from '@ngrx/store';
import { UserState } from '@ds/core/users/store/user.reducer';
import { LoadUser, UpdateUser } from '@ds/core/users/store/user.actions';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Component({
    selector: 'ds-company-app',
    templateUrl: './company-app.component.html',
    styleUrls: ['./company-app.component.scss']
})
export class CompanyAppComponent implements OnInit, OnDestroy {
    routerSubscription: Subscription;
    destroy$ = new Subject();

    constructor(
        private account: AccountService,
        @Inject(DOCUMENT) private document: Document,
        private router: Router,
    ) { }

    ngOnInit() {
        this.account.getUserInfo().subscribe();

        //this.checkMenuWrapperState();
        this.routerSubscription = this.router.events.pipe(takeUntil(this.destroy$)).subscribe(event => {
            if (event instanceof NavigationEnd) {
                //this.checkMenuWrapperState();
            }
        });
    }

    ngOnDestroy() {
        if (this.routerSubscription) this.routerSubscription.unsubscribe();
        this.destroy$.next();
    }

}
