import { Component, OnInit, AfterViewInit, AfterViewChecked, Inject } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';
import { DOCUMENT } from '@angular/common';
import { UserInfo, SiteConfiguration } from '@ds/core/shared';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map, tap, switchMap } from 'rxjs/operators';
import * as _ from "lodash";
import { Observable } from 'rxjs';
import { AccountService } from '@ds/core/account.service';

@Component({
  selector: 'ds-account-settings',
  templateUrl: './account-settings.component.html',
  styleUrls: ['./account-settings.component.scss']
})
export class AccountSettingsComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    mfa: any = {};
    siteConfig: SiteConfiguration;
    siteConfig$: Observable<SiteConfiguration>;
    isLoading = true;

    constructor(
        private styles: DsStyleLoaderService,
        private accountService: AccountService,
        private msgSvc: DsMsgService,
        @Inject(DOCUMENT) private document: any
    ) { }

    ngOnInit() {

        this.siteConfig$ = this.accountService.getUserInfo()
            .pipe(
                switchMap(user => {
                    this.user = user;
                    return this.accountService.getAccountSettings();
                }),
                switchMap(settings => this.accountService.getMfaRequirements(settings.authUserId)),
                switchMap(mfa => {
                    this.mfa = mfa;
                    return this.accountService.getSiteConfigurations();
                }),
                tap(config => {
                    this.siteConfig = config;
                    this.isLoading = false;
                })
            );
    }

    gotoQA() {
        this.document.location.href = this.siteConfig.authRootUrl + ['/mods/questions-email'];
    }

    gotoResetPasword() {
        this.document.location.href = this.siteConfig.authRootUrl + ['/mods/update-pass'];
    }

    gotoMfa() {
        this.document.location.href = this.siteConfig.authRootUrl + ['/mods/multi-factor'];
    }

    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}
