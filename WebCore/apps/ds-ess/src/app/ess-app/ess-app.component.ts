import { Component, OnInit, Inject, OnDestroy, HostListener, AfterContentInit, AfterViewInit } from '@angular/core';
import { AccountService } from '@ds/core/account.service';
import { Subscription } from 'rxjs';
import { DOCUMENT } from '@angular/common';
import { Router, NavigationEnd } from '@angular/router';
import { EssStyleService } from '../styles.service';
import { printAngularRouterPaths } from '@ds/core/utilities';
import { environment } from '../../environments/environment';
import { APP_CONFIG, AppConfig } from '@ds/core/app-config/app-config';
import { Store } from '@ngrx/store';
import { UserState } from '@ds/core/users/store/user.reducer';
import { UpdateUser } from '@ds/core/users/store/user.actions';


@Component({
    selector: 'ess-app',
    templateUrl: './ess-app.component.html',
    styleUrls: ['./ess-app.component.scss']
})
export class EssAppComponent implements OnInit, OnDestroy {
    isLoading = true;
    hasMenuWrapper = false;
    routerSubscription: Subscription;
    hasSubmenu = true;
    isOnboarding = false;
    lastNavigationUrl: string;

    constructor(
        private account: AccountService,
        @Inject(DOCUMENT) private document: Document,
        private router: Router,
        private styles: EssStyleService,
        private store: Store<UserState>,
        @Inject(APP_CONFIG) private config: AppConfig
    ) { }
    ngOnInit() {
        this.account.getUserInfo().subscribe();

        this.routerSubscription = this.router.events.subscribe(event => {
            if (event instanceof NavigationEnd) {
                this.checkMenuWrapperState();
            }
        });
    }

    ngOnDestroy() {
        if (this.routerSubscription) this.routerSubscription.unsubscribe();
    }

    @HostListener('window:locationchange', ['$event'])
    urlChangeHandler(event) {

        event.preventDefault();
        event.stopPropagation();

        const targetUrl = event.target.document.URL;

        if (this.lastNavigationUrl == null)
            this.lastNavigationUrl = targetUrl.toLowerCase();

        if (this.lastNavigationUrl !== targetUrl.toLowerCase() && !this.hasMenuWrapper) {
            this.lastNavigationUrl = targetUrl.toLowerCase();
            const siteUrl = this.config.baseSite.url;
            const ngUrl = targetUrl.replace(siteUrl, '');

            this.router.navigateByUrl(ngUrl, {skipLocationChange: true});
        }
    }

    checkMenuWrapperState() {

        this.isLoading = false;
        const url = this.document.location.href.trim().toLowerCase();
        const isOnboardingPage = url.includes('onboarding');
        this.hasMenuWrapper = !isOnboardingPage;
        this.hasSubmenu = !(url.includes('benefits') || url.includes('notes'));
        this.isOnboarding = url.includes('onboarding');

    }
}
