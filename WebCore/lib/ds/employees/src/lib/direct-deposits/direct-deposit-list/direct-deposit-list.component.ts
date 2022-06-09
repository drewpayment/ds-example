import { Component, OnInit, AfterViewInit, AfterViewChecked } from '@angular/core';
import { DsStyleLoaderService, IStyleAsset } from '@ajs/ui/ds-styles/ds-styles.service';

import { EmployeeDirectDepositsService } from '../shared/employee-direct-deposit-api.service';
import { IEmployeeDirectDepositInfo } from '../shared/employee-direct-deposit-info.model';

import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { map } from 'rxjs/operators';
import * as _ from "lodash";

@Component({
  selector: 'ds-direct-deposit-list',
  templateUrl: './direct-deposit-list.component.html',
  styleUrls: ['./direct-deposit-list.component.scss']
})
export class DirectDepositListComponent implements OnInit, AfterViewChecked {
    mainStyle: IStyleAsset;
    user: UserInfo;
    accounts: IEmployeeDirectDepositInfo[];

    constructor(
        private styles: DsStyleLoaderService,
        private service: EmployeeDirectDepositsService,
        private accountService: AccountService,
        private msgSvc: DsMsgService
    ) { }

    ngOnInit() {
        this.accounts = [],

        this.accountService.getUserInfo().subscribe(user => {
            this.user = user;

            this.service.getOnboardingAccounts(this.user.employeeId).subscribe(accounts => {
                this.accounts = accounts;
            });
        });
    }

    getAmount = function (account) {
        if (account.amountType === -3) {
            return '$' + account.amount;
        }

        if (account.amountType === -1) {
            return account.amount + '%';
        }
    };

    /**
     * We tell DsStyleLoaderService that this component should use main stylesheet AFTER OnInit and AfterViewInit
     * because we need to make sure that everything is resolved above this component. The DsStyleLoaderService is not
     * instantiated until after OutletComponent is finished loading.
     */
    ngAfterViewChecked() {
        this.styles.useMainStyleSheet();
    }
}
