import { Component, OnInit, AfterViewChecked } from '@angular/core';
import { UserInfo, UserType } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { EmployeeService } from '@ds/core/employees/employee.service';
import { ClientAlert } from '@ds/core/clients/shared/client-alert.model';
import { ActivatedRoute } from '@angular/router';
import { tap, skip, filter } from 'rxjs/operators';
import { DefaultPageSettings } from '../../models/default-page-settings.model';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { CookieMonsterService } from '@ds/core/cookie-monster/cookie-monster.service';
import { IClientWorkNumberPolicy } from '@ds/core/clients/shared';
import { WorkNumberModalComponent } from '@ds/core/popup/equifax/work-number/work-number-modal.component';
import { TermsAndConditionsModalService } from '@ds/core/ui/nav/ds-nav-help-links/terms-and-conditions/terms-and-conditions-modal.service';
import { TermsAndConditionsComponent } from '@ds/core/ui/nav/ds-nav-help-links/terms-and-conditions/terms-and-conditions.component';
import { DominionShortcut } from '../../models/dominion-shortcut.model';
import { Client } from '@ds/core/employee-services/models';
import { TurboTaxModalComponent } from '../../shared/turbo-tax-modal/turbo-tax-modal.component';
import { IAlert } from 'lib/models/src/lib/alert.model';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';

@Component({
  selector: 'ds-default',
  templateUrl: './default.component.html',
})
export class DefaultComponent implements OnInit, AfterViewChecked {
  alerts: ClientAlert[];
  messages: ClientAlert[];
  sortedAlerts: ClientAlert[];
  sortedMessages: ClientAlert[];
  shortcut: DominionShortcut;
  client: Client;
  settings: DefaultPageSettings;
  isLoading: Boolean = true;
  user: UserInfo;
  isFirstVisit: boolean = false;
  workNumber: IClientWorkNumberPolicy;
  turboTaxNeeded: Boolean = true;
  userType: UserType.systemAdmin;
  openTerms$: Observable<any>;

  constructor(
    private api: EmployeeService,
    public accountService: AccountService,
    private msg: NgxMessageService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog,
    public cookieService: CookieMonsterService,
    private termsAndCondSvc: TermsAndConditionsModalService
  ) {}

  ngOnInit() {
    this.api.data$.pipe(skip(1)).subscribe((data) => {
      this.settings = data[0];
      this.alerts = data[1].filter((x) => x.alertType == 1);
      this.sortedAlerts = this.alerts.sort((a, b) => {
        return <any>new Date(b.datePosted) - <any>new Date(a.datePosted);
      });
      this.messages = data[1].filter((x) => x.alertType == 2);
      this.sortedMessages = this.messages.sort((a, b) => {
        return <any>new Date(b.datePosted) - <any>new Date(a.datePosted);
      });

      this.workNumber = data[3];

      this.openTerms$ = TermsAndConditionsComponent.userCanAddressContract(
        this.accountService
      ).pipe(
        filter((canOpen) => canOpen),
        tap(() => this.termsAndCondSvc.open())
      );

      this.accountService.getUserInfo().subscribe((u) => {
        this.user = u;
        if (this.settings.showTurboTaxPopup) {
          let cookieValue = this.cookieService.getCookie('default_TTPopup');

          if (cookieValue == null) {
            this.cookieService.setCookie(
              'default_TTPopup',
              this.user.userId,
              true,
              null,
              1,
              null,
              null
            );
            this.isFirstVisit = true;
          } else {
            if (+cookieValue == this.user.userId) {
              this.isFirstVisit = false;
            } else {
              this.cookieService.removeCookie('default_TTPopup');
              this.isFirstVisit = true;
              this.cookieService.setCookie(
                'default_TTPopup',
                this.user.userId,
                true,
                null,
                1,
                null,
                null
              );
            }
          }
        }

        if (this.workNumber === null && this.user.userTypeId === 2) {
          this.showEquifaxModal();
        } else {
          if (this.settings.showTurboTaxPopup && this.isFirstVisit) {
            this.showTurboTaxModal();
          }
        }

        this.isLoading = false;
      });
    });

    this.api.fetchFakeResolver();
  }

  ngAfterViewChecked() {}

  showTurboTaxModal() {
    const config = new MatDialogConfig<any>();

    config.width = '800px';
    config.disableClose = true;

    const dialogRef = this.dialog.open(TurboTaxModalComponent, config);

    return dialogRef;
  }

  showEquifaxModal() {
    const config = new MatDialogConfig<any>();

    config.width = '800px';
    config.disableClose = true;
    config.panelClass = 'equifax';

    const dialogRef = this.dialog.open(WorkNumberModalComponent, config);

    dialogRef.afterClosed().subscribe((result) => {
      if (this.settings.showTurboTaxPopup && this.isFirstVisit) {
        this.showTurboTaxModal();
      }
    });

    return dialogRef;
  }

  openTurboTaxInNewWindow() {
    window.open('http://turbotax.intuit.com/affiliate/domini', '_blank');
  }

  download(alert: IAlert) {
    this.api.getFileAlertToDownload(alert).subscribe(
      (res) => {},
      (error) => {
        this.msg.setErrorMessage(error);
      }
    );
  }
}
