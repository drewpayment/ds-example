import {
    Directive, ElementRef, Input, Inject, ChangeDetectorRef, OnDestroy,
    HostListener, Renderer2, OnChanges, Optional, Injectable, SimpleChanges
} from '@angular/core';
import { DsNavMenuItemOptions, UrlParts } from '@ds/core/shared';
import { DOCUMENT, LocationStrategy, PlatformLocation, PathLocationStrategy, APP_BASE_HREF } from '@angular/common';
import { IMenuItem, ApplicationSourceType } from '@ds/core/app-config';
import { MenuService } from '@ds/core/app-config/shared/menu.service';
import { combineLatest, Subject } from 'rxjs';
import { AccountService } from '@ds/core/account.service';
import { takeUntil, take, first } from 'rxjs/operators';
import { Router, RouterLinkWithHref, ActivatedRoute } from '@angular/router';
import { LocalLinkService } from './local-link.service';
import { WrappedLocationStrategy } from './wrapped-location-strategy';
import { APP_CONFIG, AppConfig, joinWithSlash } from '@ds/core/app-config/app-config';
import { ConfigUrlType, ConfigUrl } from '@ds/core/shared/config-url.model';


@Directive({
    selector: '[dsNavMenuItem]',
    providers: [
        LocalLinkService,
    ]
})
export class DsNavMenuItemDirective extends RouterLinkWithHref implements OnDestroy, OnChanges {

    private _item: IMenuItem;
    @Input('dsNavMenuItem')
    set item(value: IMenuItem) {
        this._item = value;
        if (value && value.resource && value.resource.routeUrl) {
            this.checkRouterLink();
        }
    }

    get item(): IMenuItem {
        return this._item;
    }

    destroy$ = new Subject();
    isAngularRoute = false;
    navigateTo: string;
    onInit = true;

    private _href: string;
    private _router: Router;

    constructor(
        private el: ElementRef,
        private menuService: MenuService,
        private accountService: AccountService,
        router: Router,
        route: ActivatedRoute,
        locationStrategy: LocationStrategy,
        private localLinkService: LocalLinkService,
        @Inject(APP_CONFIG) private config: AppConfig,
    ) {
        super(router, route, new WrappedLocationStrategy(locationStrategy, localLinkService));

        this._router = router;
    }

    ngOnDestroy() {
        this.destroy$.next();
    }

    private checkRouterLink() {

        if (!this.onInit) return;
        this.onInit = false;

        combineLatest(
            this.accountService.getSiteUrls(),
            this.menuService.getNavigationHistory(),
            this.menuService.getMenu(),
            this.menuService.getSelectedItem(),
        )
            .pipe(takeUntil(this.destroy$), first())
            .subscribe(([urls, navHistory, menu, selectedItem]) => {
                const isUserCurrentlyOnEssOrCompany = this.config.baseSite.siteType === ConfigUrlType.Ess
                    || this.config.baseSite.siteType === ConfigUrlType.Company;
                const targetSiteConfig = urls.find(u => u.siteType === (this.item.resource.applicationSourceType - 1));

                this.replaceUrl = true;
                this.skipLocationChange = !(this.item && this.config.baseSite.siteType === ConfigUrlType.Company
                    && this.item.resource.applicationSourceType === ApplicationSourceType.CompanyWeb);
                this.isAngularRoute = this.item.isAngularRoute ||
                    (isUserCurrentlyOnEssOrCompany && this.item && this.item.resource
                        && (this.item.resource.applicationSourceType === ApplicationSourceType.EssWeb
                            || this.item.resource.applicationSourceType === ApplicationSourceType.CompanyWeb));

                this.isAngularRoute = !this.item.resource.routeUrl.includes('benefits') || this.config.baseSite.siteType === ConfigUrlType.Company ;

                if (selectedItem != null && this.item != null && selectedItem.menuItemId === this.item.menuItemId) {
                    this.el.nativeElement.classList.add('active');
                }

                if (this.isAngularRoute && isUserCurrentlyOnEssOrCompany) {
                    if (this.config.baseSite.siteType === targetSiteConfig.siteType) {
                        const hasTrailingSlash = targetSiteConfig.url.charAt(targetSiteConfig.url.length - 1) === '/';
                        let url = this.item.resource.routeUrl.replace(targetSiteConfig.url, '').toLowerCase();

                        if (hasTrailingSlash) url = url + '/';

                        this.isAngularRoute = true;

                        this.routerLink = `/${url}`;
                        this.href = this._href = this.item.resource.routeUrl;
                    } else {
                        const urlParts = new UrlParts(targetSiteConfig.url)
                            .addPath(this.item.resource.routeUrl.replace(targetSiteConfig.url, '').toLowerCase());

                        const url = urlParts.href;
                        this.isAngularRoute = false;
                        this.routerLink = url;
                        this.href = this._href = url;
                    }

                    this.replaceUrl = true;
                    this.skipLocationChange = false;

                } else {
                    this.routerLink = this.href = this._href = this._item.resource.routeUrl.includes('http')
                        ? this._item.resource.routeUrl : this.parseTrailingSlash(targetSiteConfig.url) + this.item.resource.routeUrl;
                }

                this.localLinkService.baseHref = this._href;

                this.destroy$.next();
            });
    }

    @HostListener('click', [
        '$event.button',
        '$event.ctrlKey',
        '$event.metaKey',
        '$event.shiftKey'
    ])
    onClick(
        button: number,
        ctrlKey: boolean,
        metaKey: boolean,
        shiftKey: boolean
    ): boolean {

        if (button !== 0 || ctrlKey || metaKey || shiftKey) {
            return true;
        }

        if (typeof this.target === 'string' && this.target !== '_self') {
            return true;
        }

        /**
         * Checks to see if the link is an external link. If it is, we programmatically
         * navigate the user to the correct destination.
         */
        if (!this.localLinkService.isLocalLink((this as any).commands)) {
            (window as any).location = this._href;
            return false;
        }

        const extras = {
            skipLocationChange: attrBoolValue(this.skipLocationChange),
            replaceUrl: attrBoolValue(this.replaceUrl),
        };

        this._router.navigateByUrl(this.urlTree, extras);
        return false;
    }

    @HostListener('click', ['$event'])
    onClickEvent (
        $event: Event
    ) {
        $event.stopPropagation();
    }

    private parseTrailingSlash(url: string): string {
        return url && url.charAt(url.length - 1) === '/'
            ? url : url + '/';
    }

}

function attrBoolValue(s: any): boolean {
    return s === '' || !!s;
}
