import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { inject } from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';
import { Inject } from '@angular/core';
import { element } from 'protractor';
import { TermsAndConditionsModalService } from './terms-and-conditions/terms-and-conditions-modal.service';
import { Subject, Observable } from 'rxjs';
import { takeUntil, map } from 'rxjs/operators';

@Component({
    selector: 'ds-nav-help-links',
    templateUrl: './ds-nav-help-links.component.html',
    styleUrls: ['./ds-nav-help-links.component.scss']
})
export class DsNavHelpLinksComponent implements OnInit, OnDestroy {

    @Input()
    isSidenavOpen = true;

    @Input()
    isSideNavDrilled = false;

    showTermsAndConditions = true;
    userTypeId: number;
    isSysAdmin = false;
    isCompanyAdmin = false;
    displayWorkNumber = false;
    destroy$ = new Subject();

    constructor(
        private accountService: AccountService,
        private termsAndCondSvc: TermsAndConditionsModalService
    ) {
     }

    ngOnInit() {
        this.accountService.getUserInfo().pipe(takeUntil(this.destroy$))
            .subscribe(user => {
                this.userTypeId = user.userTypeId;

                this.isSysAdmin = this.userTypeId == UserType.systemAdmin;
                this.isCompanyAdmin = this.userTypeId == UserType.companyAdmin;
            });
    }

    ngOnDestroy() {
        this.destroy$.next();
    }

    openDialog() {
        this.termsAndCondSvc.open();
    }
}
