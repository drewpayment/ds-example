import { Component, HostListener, Inject, OnDestroy, OnInit } from '@angular/core';
import { AppConfig, APP_CONFIG } from '@ds/core/app-config/app-config';
import { Router, ActivatedRoute } from '@angular/router';
import { DOCUMENT } from '@angular/common';

@Component({
    selector: 'ds-onboarding-ess-outlet',
    templateUrl: './onboarding-outlet.component.html',
    styleUrls: ['./onboarding-outlet.component.scss'],
    host: {
        'class': 'onboarding',
    }
})
export class OnboardingOutletComponent implements OnDestroy, OnInit {

    lastNavigationUrl: string;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        @Inject(APP_CONFIG) private config: AppConfig,
        @Inject(DOCUMENT) private document: Document,
    ) {
        this.router.onSameUrlNavigation = 'reload';
        this.lastNavigationUrl = this.document.location.href;
    }

    ngOnInit() {
        // this.router.events.subscribe(event => console.dir(event));
    }

    ngOnDestroy() {
        this.router.onSameUrlNavigation = 'ignore';
    }

    // @HostListener('window:locationchange', ['$event'])
    // urlChangeHandler(event) {
    //     const target = event.target.location;
    //     console.dir(target);

    //     if (target.href !== this.lastNavigationUrl) {
    //         this.lastNavigationUrl = target.href;
    //         this.router.navigateByUrl(target.href.replace(target.origin), { replaceUrl: true });
    //     }
    // }

}
