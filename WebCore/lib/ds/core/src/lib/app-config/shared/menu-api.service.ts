import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IMenu } from './menu.model';
import { map, switchMap, tap, filter } from 'rxjs/operators';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { ApplicationSourceType, ApplicationResourceType, IApplicationResource, IMenuItem } from '.';
import { AccountService } from '@ds/core/account.service';
import { ConfigUrlType, ConfigUrl } from '@ds/core/shared/config-url.model';
import { ActivatedRoute } from '@angular/router';
import { DOCUMENT } from '@angular/common';
import { UrlParts } from '@ds/core/shared';
import { Store } from '@ngrx/store';
import { Ess_Profile_Submenu, Ess_Performance_Submenu } from './ess-submenus';
import { initializeMenu } from './menu-helpers';
import * as fromReducer from '../ngrx-store/reducers';
import { SetMenu } from '../ngrx-store/actions/menu.actions';
import { MenuService } from './menu.service';

export enum SiteDesignationType {
    source,
    ess,
    company
}

export interface MenuOptions {
    userId?: number;
    clientId?: number;
    siteDesignationType?: SiteDesignationType;
}

const PRODUCT_TYPES = {
    PROFILE: 'profile',
    PERFORMANCE: 'performance'
};

const CHECK_DATE_PAGES = [
    'selectpayrollhistory.aspx',
    'yournextpayroll.aspx',
    'payrollcontroltotal.aspx',
    'payrollpreview.aspx',
    'companyremotechecks.aspx',
    'paycheckcalculatornew.aspx',
    'payrollpaydatadetail.aspx',
    'payrollpaydatagrid.aspx',
    'payrolladjustments.aspx',
    'payrolladjustmentsgrid.aspx',
    'payrollthirdpartyadjustments.aspx',
    'payrollsuccessorwageadjustment.aspx',
    'vendoradjustments.aspx',
    'payrolladjustmentsvoidchecks.aspx',
    'paycheckcalculator.aspx',
    'payrollchecks.aspx'
];

const EMPLOYEE_SELECTOR_PAGES = [
    'employee.aspx',
    'employeepay.aspx',
    'changeemployee.aspx',
    'employeetaxes.aspx',
    'emdeduction.aspx',
    'employeecostcenter.aspx',
    'employeeaccrual.aspx',
    'certifyi9.aspx',
    'employeeexport.aspx',
    'clockemployeepunchhistory.aspx',
    'clockemployeepunchcalendar.aspx',
    'clockemployeepunch.aspx',
    'employeew2consent.aspx',
    'payrolladjustments.aspx',
    'payrolladjustmentsvoidchecks.aspx',
    'payrollsuccessorwageadjustment.aspx',
    'payrollthirdpartyadjustments.aspx',
    'employeenotes.aspx',
    'employeeattachments.aspx',
    'employeeotherinfo.aspx',
    'employeedependents.aspx',
    'employeeemergencycontact.aspx',
    'employeeevent.aspx',
    'employeepoints.aspx',
    'employeeperformance.aspx',
    'clockemployee.aspx',
    'payrollpaydatadetail.aspx',
    'benefithome.aspx',
    'manage/taxes'
];

@Injectable({
    providedIn: 'root'
})
export class MenuApiService {
    API_BASE = 'api/menus';
    siteUrls: ConfigUrl[];

    private _selectedItem: IMenuItem;
    private selectedItem$ = new BehaviorSubject<IMenuItem>(null);
    get selectedItem(): Observable<IMenuItem> {
        return this.selectedItem$.asObservable();
    }
    changeSelectedItem(item: IMenuItem) {
        this.selectedItem$.next(item);
    }

    private _homeUrl = new BehaviorSubject<string>('default.aspx');
    get homeUrl$(): Observable<string> {
        return this._homeUrl.asObservable();
    }
    showEmployeeSelector$ = new BehaviorSubject<boolean>(false);
    showCheckDate$ = new BehaviorSubject<boolean>(false);

    constructor(
        private http: HttpClient,
        private account: AccountService,
        @Inject(DOCUMENT) private document: Document,
        private menuService: MenuService
    ) {}

    /**
     * GETs the current user's menu based on roles and access.
     * @param options (Optional) If not specified the current
     * user and selected client will be used.
     */
    getCurrentMenu(options: MenuOptions = {} as MenuOptions): Observable<IMenu> {
        const params = <any>{};

        if (options.userId)
            params.userId = options.userId.toString();
        if (options.clientId)
            params.clientId = options.clientId.toString();

        return this.http.get<IMenu>(`${this.API_BASE}/current`, { params: params })
            .pipe(
                switchMap(menu => this.adaptBenefitsMenuItem(menu)),
                initializeMenu(),
                tap(menu => {
                    this.setViewPermissions(null, true);
                    this.menuService.updateMenu(menu);
                }),
            );
    }

    private adaptBenefitsMenuItem(menu: IMenu): Observable<IMenu> {
        return this.account.getSiteUrls()
            .pipe(
                tap(sites => {
                    this.siteUrls = sites;
                    const payroll = this.siteUrls.find(s => s.siteType === ConfigUrlType.Payroll);

                    if (payroll && payroll.url) {
                        this._homeUrl.next(`${payroll.url}`);
                    }
                }),
                switchMap(() => {
                    if (!menu || !menu.items || !menu.items.length) return of(menu);

                    menu.items.forEach(m => {
                        const title = m.title.trim().toLowerCase();

                        if (title === 'benefits' || title === 'notes') {
                            const resourceUrlBase = this.getBaseUrlByApplicationSourceType(ApplicationSourceType.EssWeb);
                            if (resourceUrlBase) {
                                m.resource.routeUrl = `${resourceUrlBase}${title}`;
                            } else {
                                m.resource.routeUrl = `${title}`;
                            }
                        }
                    });

                    return of(menu);
                })
            );
    }

    /**
     * Handles applications specific changes necessary to get the menu wrapper
     * to match consistently with the dominion payroll site.
     */
    private buildPayrollMenu(menu: IMenu): Observable<IMenu> {
        return this.account.getSiteUrls()
            .pipe(
                tap(urls => this.siteUrls = urls),
                map(sites => {
                    const source = sites.find(s => s.siteType === ConfigUrlType.Payroll);
                    const url = this.document.location.href;

                    menu.items.forEach(m => {
                        const title = m.title.trim().toLowerCase();

                        if (title === PRODUCT_TYPES.PERFORMANCE) {

                            for (let i = 0; i < m.items.length; i++) {
                                const item = m.items[i];
                                // item.parentId = m && m.menuItemId > 0 ? m.menuItemId : null;

                                // if (!item.resource || !item.resource.routeUrl) continue;

                                // const urlParts = UrlParts.ParseUrl(url);
                                // const itemParts = UrlParts.ParseUrl(item.resource.routeUrl);

                                // item.isActive = urlParts.isEqualTo(itemParts);

                                if (item.isActive) {
                                    // this._selectedItem = item;
                                    this.setViewPermissions(item.resource.routeUrl);
                                }
                            }
                        } else if (title === PRODUCT_TYPES.PROFILE) {
                            const items = [...Ess_Profile_Submenu];
                            const ess = sites.find(s => s.siteType === ConfigUrlType.Ess);

                            items.forEach((item, i, aa) => {
                                // aa[i].parentId = m && m.menuItemId > 0 ? m.menuItemId : null;

                                if (aa[i].resource && aa[i].resource.routeUrl && aa[i].isAngularRoute
                                    && aa[i].resource.applicationSourceType === ApplicationSourceType.EssWeb) {
                                    aa[i].resource.routeUrl = this.sanitizeUrl(`${ess.url}/${aa[i].resource.routeUrl}`);

                                    // aa[i].isActive = url.toLowerCase().includes(aa[i].resource.routeUrl.toLowerCase());
                                    if (aa[i].isActive)
                                        this.setViewPermissions(aa[i].resource.routeUrl);
                                }

                                // if the menu item is "Notifications" title, we need to specify the ASPX page
                                if (aa[i].menuItemId === 6 &&
                                    !aa[i].isAngularRoute &&
                                    aa[i].resource.applicationSourceType === ApplicationSourceType.SourceWeb
                                ) {
                                    aa[i].resource.routeUrl = `${source.url}/ContactNotificationPreferences.aspx`;
                                }
                            });
                            m.items = items;
                        }
                    });

                    return menu;
                })
            );
    }

    private sanitizeUrl(url: string): string {
        return url.replace(/([^:]\/)\/+/g, '$1');
    }

    private buildCompanyMenu(menu: IMenu): Observable<IMenu> {
        return this.account.getSiteUrls()
            .pipe(
                tap(sites => this.siteUrls = sites),
                map(_ => {
                    if (menu.items && menu.items.length) {
                        for (let i = 0; i < menu.items.length; i++) {
                            menu.items[i].items = this.setPayrollSiteUrls(menu.items[i].items);
                        }
                    }

                    return menu;
                })
            );
    }

    private setPayrollSiteUrls(items: IMenuItem[]): IMenuItem[] {
        const payrollSiteUrl = this.siteUrls.find(s => s.siteType === ConfigUrlType.Payroll);
        if (payrollSiteUrl.url.charAt(payrollSiteUrl.url.length - 1) !== '/') {
            payrollSiteUrl.url = payrollSiteUrl.url + '/';
        }

        for (let i = 0; i < items.length; i++) {
            const item = items[i];

            if (item.items && item.items.length) {
                item.items = this.setPayrollSiteUrls(item.items);
            }

            if (!item.isHidden && item.resource &&
                item.resource.applicationSourceType === ApplicationSourceType.SourceWeb &&
                item.resource.routeUrl.trim().toLowerCase().includes('.aspx')) {
                item.resource.routeUrl = payrollSiteUrl.url + item.resource.routeUrl;
            }
        }

        return items;
    }

    setViewPermissions(url: string, useBrowser: boolean = false) {
        if (useBrowser) {
            url = this.document.location.href;
        } else {
            url = url || this._selectedItem.resource.routeUrl;
        }

        this.setCheckDateViewPermission(url, useBrowser);
        this.setEmployeeSelectorPermission(url, useBrowser);
    }

    /**
     * Determines whether or not the user can see the checkdate in the top menu toolbar
     */
    private setCheckDateViewPermission(url: string, isAspx = false) {
        if (this.showCheckDate$.value) return;

        const urlParts = UrlParts.ParseUrl(url);
        const found = CHECK_DATE_PAGES.findIndex(p =>
            isAspx ? urlParts.isAspxMatch(p) : urlParts.isEqualTo(p)) > -1;

        if (found !== this.showCheckDate$.value)
            this.showCheckDate$.next(found);
    }

    /**
     * Determines whether or not the user can see the employee selector in the top menu toolbar
     */
    private setEmployeeSelectorPermission(url: string, isAspx = false) {
        if (this.showEmployeeSelector$.value) return;

        const urlParts = UrlParts.ParseUrl(url);
        const found = EMPLOYEE_SELECTOR_PAGES.findIndex(p =>
            isAspx ? urlParts.isAspxMatch(p) : urlParts.isEqualTo(p)) > -1;

        if (found !== this.showEmployeeSelector$.value)
            this.showEmployeeSelector$.next(found);
    }

    private buildEssMenu(menu: IMenu): Observable<IMenu> {
        return this.account.getSiteUrls()
            .pipe(
                map(sites => {
                    const payrollSiteUrl = sites.find(s => s.siteType === ConfigUrlType.Payroll);
                    const url = this.document.location.href;

                    // loop over top-level product titles
                    // MENU IS ENTIRE API OBJECT RETURNED
                    menu.items.forEach((m, index, arr) => {
                        const productTitle = m.title.trim().toLowerCase();
                        if (productTitle === PRODUCT_TYPES.PROFILE) {
                            // tslint:disable-next-line: no-use-before-declare
                            const items = [...Ess_Profile_Submenu];
                            items.forEach((item, i, aa) => {
                                aa[i].parentId = m && m.menuItemId > 0 ? m.menuItemId : null;

                                if (aa[i].resource && aa[i].resource.routeUrl
                                    && aa[i].resource.applicationSourceType === ApplicationSourceType.EssWeb) {
                                    aa[i].isActive = url.toLowerCase().includes(aa[i].resource.routeUrl.toLowerCase());

                                    if (aa[i].isActive)
                                        this.setViewPermissions(aa[i].resource.routeUrl);
                                }

                                // if the menu item is "Notifications" title, we need to specify the ASPX page
                                if (aa[i].menuItemId === 6 &&
                                    !aa[i].isAngularRoute &&
                                    aa[i].resource.applicationSourceType === ApplicationSourceType.SourceWeb
                                ) {
                                    aa[i].resource.routeUrl = `${payrollSiteUrl.url}/ContactNotificationPreferences.aspx`;
                                }
                            });
                            m.items = items;
                        } else if (productTitle === PRODUCT_TYPES.PERFORMANCE) {
                            m.resource.routeUrl = null;

                            // tslint:disable-next-line: no-use-before-declare
                            const items = [...Ess_Performance_Submenu];
                            items.forEach(item => {
                                item.parentId = m && m.menuItemId > 0 ? m.menuItemId : null;

                                if (item.resource && item.resource.routeUrl
                                    && item.resource.applicationSourceType === ApplicationSourceType.EssWeb) {
                                    item.isActive = url.toLowerCase().includes(item.resource.routeUrl.toLowerCase());

                                    if (item.isActive) {
                                        this.setViewPermissions(item.resource.routeUrl);
                                    }
                                }
                            });
                            m.items = items;
                        } else {
                            const items = [...m.items];
                            m.items = this.updateUrlsWithBaseSiteContext(items);
                        }
                    });

                    return menu;
                })
            );
    }

    private updateUrlsWithBaseSiteContext(items: IMenuItem[]): IMenuItem[] {
        const payrollSiteUrl = this.siteUrls.find(s => s.siteType === ConfigUrlType.Payroll);
        const essSite = this.siteUrls.find(s => s.siteType === ConfigUrlType.Ess);

        for (let i = 0; i < items.length; i++) {
            const item = items[i];

            if (item.items && item.items.length) {
                item.items = this.updateUrlsWithBaseSiteContext(item.items);
            } else {

                if (item.resource && item.resource.applicationSourceType === ApplicationSourceType.SourceWeb) {
                    item.resource.routeUrl = `${payrollSiteUrl.url}/${item.resource.routeUrl}`;
                } else if (item.resource && item.resource.applicationSourceType === ApplicationSourceType.EssWeb) {
                    item.resource.routeUrl = `${essSite.url}/${item.resource.routeUrl}`;
                }
            }
        }

        return items;
    }

    private getBaseUrlByApplicationSourceType(source: ApplicationSourceType): string {
        switch (source) {
            case ApplicationSourceType.EssWeb:
                return this.siteUrls.find(u => u.siteType === ConfigUrlType.Ess).url;
            case ApplicationSourceType.CompanyWeb:
                return this.siteUrls.find(u => u.siteType === ConfigUrlType.Company).url;
            case ApplicationSourceType.SourceWeb:
            default:
                return this.siteUrls.find(u => u.siteType === ConfigUrlType.Payroll).url;
        }
    }

}

