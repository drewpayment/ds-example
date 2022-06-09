import { Component, OnInit, Input } from '@angular/core';
import { EmployeeApiService } from '@ds/core/employees/shared/employee-api.service';
import * as moment from 'moment';
import { DsConfirmService } from '@ajs/ui/confirm/ds-confirm.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { tap, switchMap } from 'rxjs/operators';
import { AccountService } from '@ds/core/account.service';
import { Observable, throwError } from 'rxjs';
import { coerceNumberProperty,coerceBooleanProperty } from '@angular/cdk/coercion';
import { IElectronicConsents } from '@ds/core/employees/shared/electronic-consents.model';
import { UserInfo } from '@ds/core/shared';
import { DsConfigurationService } from "@ajs/core/ds-configuration/ds-configuration.service";
import { EmployeeSearchService } from "@ajs/employee/search/shared/employee-search.service";

@Component({
  selector: 'ds-electronic-consents',
  templateUrl: './electronic-consents.component.html',
  styleUrls: ['./electronic-consents.component.scss'],
  providers: [],
})
export class ElectronicConsentsComponent implements OnInit {
  private _employeeId = 0;
  @Input()
  get employeeId(): number {
    return this._employeeId;
  }
  set employeeId(value: number) {
      this._employeeId = coerceNumberProperty(value);
  }

  isHRBlocked:          boolean = false;
  ViewOnly = false;
  userinfo:             UserInfo;
  loading:              boolean;
  readonly:             boolean;
  selected:             string;
  subscriptionHandler$: Observable<any>;
  elecronicConsent:     IElectronicConsents;

  private _isEssMode = false;
  @Input()
  get isEssMode(): boolean {
      return this._isEssMode;
  }
  set isEssMode(value: boolean) {
      this._isEssMode = coerceBooleanProperty(value);
  }

  constructor(
    private api: EmployeeApiService,
    private searchService: EmployeeSearchService,
    private confirmSvc: DsConfirmService,
    private msgSvc: DsMsgService,
    private accountService: AccountService,
    private dsConfig: DsConfigurationService,
  ) { }


  ngOnInit() {
    this.selected = "W2";
    this.loading = true;

    this.subscriptionHandler$ = this.accountService.getUserInfo().pipe( tap( data => {
      this.userinfo = data;
        if(!this.employeeId) {
          if(this.isEssMode)  this.employeeId = this.userinfo.userEmployeeId;
          else                this.employeeId = this.userinfo.lastEmployeeId || this.userinfo.employeeId;
        }
        this.readonly = (!this.isEssMode && (!this.userinfo.userEmployeeId || this.employeeId != this.userinfo.userEmployeeId) );
      }), switchMap( data => this.api.getEmployeeConsentData(this.employeeId)));


      this.subscriptionHandler$.subscribe( data => {
        this.elecronicConsent = data;
        this.loading = false;
      })

    this.api.getCurrentUserHRBlockedAndViewOnly().subscribe( x => {
      //this.isHRBlocked = x.isHrBlocked;
      this.ViewOnly = x.isEmployeeSelfServiceViewOnly;
    });

    // Employee Selection changed
    if(!!this.searchService.hookToEmployeeChanged){
      this.searchService.hookToEmployeeChanged((newEmpId: number)=>{
        this.employeeId = newEmpId;
        this.loading = true;
        // update the Principal Dominion object with selected EmployeeId
        (<HTMLInputElement>document.querySelector("input[id$='hdnEmployeeId']")).value = newEmpId.toString();
        (<HTMLInputElement>document.querySelector("input[id$='btnRefreshPage']")).click();

        this.api.getEmployeeConsentData(this.employeeId).subscribe(data => {
          this.elecronicConsent = data;
          this.readonly = (!this.isEssMode && (!this.userinfo.userEmployeeId || this.employeeId != this.userinfo.userEmployeeId) );
          this.loading = false;
        },(err)=>{
            this.loading = false;
            if(err.error.errors && err.error.errors.length > 0)
              this.msgSvc.setTemporaryMessage(err.error.errors[0].msg, this.msgSvc.messageTypes.error, 6000);
            else
              this.msgSvc.showWebApiException(err.error);
            return throwError("Error Fetching Consent data");
        });
      });
    }
  }

  gotoPreferences = () => {
    let  notificationUrl = this.dsConfig.getAbsoluteUrl('Legacy/ContactNotificationPreferences.aspx');
    window.location.href = notificationUrl;
  }

  acceptedDate = () => {
    if(this.selected == "W2"){
      return !!this.elecronicConsent.consentDateW2 ?
        moment(this.elecronicConsent.consentDateW2).format('MM/DD/YYYY') : null;
    } else {
      return !!this.elecronicConsent.consentDate1095C ?
        moment(this.elecronicConsent.consentDate1095C).format('MM/DD/YYYY') : null;
    }
  }

  withdrawnDate = () => {
    if(this.selected == "W2"){
      return !!this.elecronicConsent.withdrawalDateW2 ?
        moment(this.elecronicConsent.withdrawalDateW2).format('MM/DD/YYYY') : null;
    } else {
      return !!this.elecronicConsent.withdrawalDate1095C ?
        moment(this.elecronicConsent.withdrawalDate1095C).format('MM/DD/YYYY') : null;
    }
  }

  showAccept = ():boolean => {
    let accDate = this.acceptedDate();
    let wdDate = this.withdrawnDate();

    if(!accDate) return true;
    else if(!!accDate && !!wdDate) return true;
    return false;
  }

  accept(){
    var today = new Date();
    this.elecronicConsent.hasOnlyW2 = false;
    this.elecronicConsent.hasOnly1095C = false;
    this.elecronicConsent.modified = today;
    this.elecronicConsent.modifiedBy = this.userinfo.userId;

    if(this.selected == "W2"){
      this.elecronicConsent.hasOnlyW2 = true;
      this.elecronicConsent.consentDateW2 = today;
      this.elecronicConsent.withdrawalDateW2 = null;
    } else {
      this.elecronicConsent.hasOnly1095C = true;
      this.elecronicConsent.consentDate1095C = today;
      this.elecronicConsent.withdrawalDate1095C = null;
    }

    this.api.updateEmployeeConsent(this.elecronicConsent).subscribe(x => {
      this.elecronicConsent = x;
    }, err => {
      this.msgSvc.showWebApiException(err);
    });
  }

  withdraw(){
    this.elecronicConsent.hasOnlyW2 = false;
    this.elecronicConsent.hasOnly1095C = false;
    var today = new Date();

    if(this.selected == "W2"){
      this.elecronicConsent.hasOnlyW2 = true;
      this.elecronicConsent.withdrawalDateW2 = today;
    } else {
      this.elecronicConsent.hasOnly1095C = true;
      this.elecronicConsent.withdrawalDate1095C = today;
    }

    this.api.updateEmployeeConsent(this.elecronicConsent).subscribe(x => {
      this.elecronicConsent = x;
    }, err => {
      this.msgSvc.showWebApiException(err);
    });
  }
}
