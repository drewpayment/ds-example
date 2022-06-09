import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { BillingService } from '../services/billing.service';
import { HttpErrorResponse } from '@angular/common/http';
import { FeatureOptionGroup } from '@models';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-feature',
  templateUrl: './feature.component.html',
  styleUrls: ['./feature.component.scss'],
})
export class FeatureComponent implements OnInit {
  searchText = new FormControl('');
  searchLength: number = 0;
  maxLength: number = 0;

  user: UserInfo;
  featureGroups: FeatureOptionGroup[];
  hr: FeatureOptionGroup;
  integrations: FeatureOptionGroup;
  reporting: FeatureOptionGroup;
  tna: FeatureOptionGroup;
  payroll: FeatureOptionGroup;

  isLoading = true;
  isSaving = false;
  constructor(
    private accountService: AccountService,
    private billingService: BillingService,
    private ngxMsgSvc: NgxMessageService
  ) {}

  ngOnInit() {
    this.accountService
      .getUserInfo()
      .pipe(tap((u) => (this.user = u)))
      .subscribe(() => {
        this.getClientFeatures();
      }); // end of getUserInfo

    this.searchText.valueChanges
      .pipe(debounceTime(250), distinctUntilChanged())
      .subscribe((val) => {
        this.filterOptions(val);
      }); // end of search changed
  } // end of init

  save() {
    this.isSaving = true;
    this.billingService
      .saveClientFeatures(this.user.clientId, this.featureGroups)
      .subscribe(
        () => {
          this.accountService.getAllowedActions(true).subscribe();
          this.getClientFeatures();
          this.ngxMsgSvc.setSuccessMessage(
            'Client Features saved successfully.'
          );
        },
        (error: HttpErrorResponse) => {
          this.ngxMsgSvc.setErrorMessage(error.message);
        }
      );
  }

  getClientFeatures() {
    this.billingService
      .getClientFeatures(this.user.clientId)
      .subscribe((res) => {
        this.featureGroups = res;

        this.hr = { ...this.featureGroups[0] };
        this.integrations = { ...this.featureGroups[1] };
        this.payroll = { ...this.featureGroups[2] };
        this.reporting = { ...this.featureGroups[3] };
        this.tna = { ...this.featureGroups[4] };

        this.maxLength = this.getMaxLength();
        this.searchLength = this.maxLength;

        this.isSaving = false;
        this.isLoading = false;
      }); // end of getClientFeatures
  }

  filterOptions(val: string) {
    let newSearchLength = 0;
    const featureGroups = this.featureGroups;
    for (let i = 0; i < featureGroups.length; i++) {
      let filter = featureGroups[i].featureOptions.filter((x) =>
        x.description.trim().toLowerCase().includes(val.trim().toLowerCase())
      );
      newSearchLength = newSearchLength + filter.length;

      switch (i) {
        case 0:
          this.hr.featureOptions = filter;
          break;
        case 1:
          this.integrations.featureOptions = filter;
          break;
        case 2:
          this.payroll.featureOptions = filter;
          break;
        case 3:
          this.reporting.featureOptions = filter;
          break;
        case 4:
          this.tna.featureOptions = filter;
          break;
        default:
          break;
      } // end of switch
    } // end of for
    this.searchLength = newSearchLength;
  } // end of filter method

  getMaxLength(): number {
    let maxLength = 0;

    maxLength = maxLength + this.hr.featureOptions.length;
    maxLength = maxLength + this.integrations.featureOptions.length;
    maxLength = maxLength + this.payroll.featureOptions.length;
    maxLength = maxLength + this.reporting.featureOptions.length;
    maxLength = maxLength + this.tna.featureOptions.length;

    return maxLength;
  }

  getBillingCount(count: number): string {
    return count > 1 ? count + ' Billing Items' : '1 Billing Item';
  }
}
