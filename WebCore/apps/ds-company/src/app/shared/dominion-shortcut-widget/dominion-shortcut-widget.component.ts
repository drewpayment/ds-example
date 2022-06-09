import { Component, OnInit, Input } from '@angular/core';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { HttpParams, HttpRequest } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { tap, skip } from 'rxjs/operators';
import { MatDialogConfig, MatDialog } from '@angular/material/dialog';
import { NoPaycheckModalComponent } from '../no-paycheck-modal/no-paycheck-modal.component';
import { EmployeeService } from '@ds/core/employees/employee.service';
import { DominionShortcut } from '../../models/dominion-shortcut.model';
import { A } from '@angular/cdk/keycodes';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Component({
  selector: 'ds-dominion-shortcut-widget',
  templateUrl: './dominion-shortcut-widget.component.html',
  styleUrls: ['./dominion-shortcut-widget.component.scss']
})
export class DominionShortcutWidgetComponent implements OnInit {

  user: UserInfo;
  ds: DominionShortcut;
  isLoading = false;
  payrollUrl: string;

  constructor(
    private api: EmployeeService,
    private accountService : AccountService,
    private DsPopup: DsPopupService,
    private activatedRoute: ActivatedRoute,
    public dialog: MatDialog) { }

  ngOnInit() {

    this.api.data$.subscribe(data => {
      if (data == null) return;

      this.ds = data[2];

      this.accountService.getUserInfo().pipe(tap(u => this.user = u)).subscribe(() => {
          this.isLoading = false;
      });
    });

    this.accountService.getSiteUrls().subscribe(urls => {
      const payroll = urls.find((u) => u.siteType === ConfigUrlType.Payroll);
      this.payrollUrl = payroll.url;
    });
  }

  getPaycheck() {

    if (this.ds.genPaycheckHistoryId != 0) {
      let w = window,
      d = document,
      e = d.documentElement,
      g = d.getElementsByTagName('body')[0],
      x = w.innerWidth || e.clientWidth || g.clientWidth,
      y = w.innerHeight|| e.clientHeight|| g.clientHeight;

      const urlBuilder = `${this.payrollUrl}api/payroll/check-history/${this.user.userEmployeeId}/check-report/${-1}`;
      this.DsPopup.open(urlBuilder, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });
    } else {
      this.showNoPaycheck();
    }
  }

  getW2() {
    let w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName('body')[0],
    x = w.innerWidth || e.clientWidth || g.clientWidth,
    y = w.innerHeight|| e.clientHeight|| g.clientHeight;

    const urlBuilder            = `${this.payrollUrl}api/payroll/reports/standard/${-55}`;
    let params                  = new HttpParams();
    params                      = params.append('clientId', this.user.clientId.toString());
    params                      = params.append('employeeId', this.user.userEmployeeId.toString());
    let request                 = new HttpRequest("GET", urlBuilder, {params: params}).urlWithParams;
    this.DsPopup.open(request, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });

  }

  getAca1095C() {
    let w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName('body')[0],
    x = w.innerWidth || e.clientWidth || g.clientWidth,
    y = w.innerHeight|| e.clientHeight|| g.clientHeight;

    const urlBuilder            = `${this.payrollUrl}api/client/reports/1095C`;
    let params                  = new HttpParams();
    params                      = params.append('employeeId', this.user.userEmployeeId.toString());
    let request                 = new HttpRequest("GET", urlBuilder, {params: params}).urlWithParams;
    this.DsPopup.open(request, '_blank', { height: y / 1.25, width: x / 1.25, top: 0, left: 0 });

  }

  changeUrl(url: string) {
    window.location.href = url;
  }

  showNoPaycheck() {
    const config        = new MatDialogConfig<any>();
    config.width        = '400px';
    config.disableClose = true;

    const dialogRef = this.dialog.open(NoPaycheckModalComponent, config);

    return dialogRef;
  }

}
