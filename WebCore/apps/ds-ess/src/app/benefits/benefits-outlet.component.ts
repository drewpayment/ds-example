import { Component, OnInit, HostListener, Inject } from '@angular/core';
import { AppConfig, APP_CONFIG } from '@ds/core/app-config/app-config';
import { Router } from '@angular/router';


@Component({
    selector: 'ess-benefits-outlet',
    templateUrl: './benefits-outlet.component.html',
    styleUrls: ['./benefits-outlet.component.scss']
})
export class BenefitsOutletComponent implements OnInit {

    lastNavigationUrl: string;
    constructor(@Inject(APP_CONFIG) private config: AppConfig, private router: Router) {}

    ngOnInit() {}

    // @HostListener('window:locationchange', ['$event'])
    // urlChangeHandler(event) {
    //     const targetUrl = event.target.document.URL;

    //     if (this.lastNavigationUrl !== targetUrl.toLowerCase()) {
    //         this.lastNavigationUrl = targetUrl.toLowerCase();
    //         const siteUrl = this.config.baseSite.url;
    //         const ngUrl = targetUrl.replace(siteUrl, '');

    //         this.router.navigateByUrl(ngUrl);
    //     }
    // }

}
