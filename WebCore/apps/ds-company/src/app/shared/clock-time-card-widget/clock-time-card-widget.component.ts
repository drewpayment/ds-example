import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../../../../../../lib/ds/core/src/lib/employees/employee.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { ActivatedRoute } from '@angular/router';
import { ClockTimeCard } from '../../../../../ds-source/src/app/employee/shared/models/clock-client-time-card.model';
import { MatTableDataSource } from '@angular/material/table';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { skip, switchMap, tap } from 'rxjs/operators';
import { PopupService } from '@ds/core/popup/popup.service';
import { NgxMessageService } from '@ds/core/ngx-message/ngx-message.service';
import { Subscription } from 'rxjs';
import { ConfigUrlType } from '@ds/core/shared/config-url.model';

@Component({
  selector: 'ds-clock-time-card-widget',
  templateUrl: './clock-time-card-widget.component.html',
  styleUrls: ['./clock-time-card-widget.component.scss']
})
export class ClockTimeCardWidgetComponent implements OnInit {

  user: UserInfo;
  matList: any;
  rows: ClockTimeCard[];
  displayedColumns: string[] = ['date', 'punches', 'hours','schedule','exceptions','notes'];
  firstDate: String;
  isLoading = true;
  employeeCostCenterId: number;
  popup: Subscription;

  constructor(
    private api : EmployeeService,
    public accountService : AccountService,
    private msg : NgxMessageService,
    private activatedRoute: ActivatedRoute,
    public clockService: ClockService,
    private DsPopup: PopupService

  ) { }

  ngOnInit() {
    // getData$
    this.accountService.getUserInfo().pipe(tap(u => this.user = u)).subscribe(() => {
      this.api.getEmployeeWorkInfo(this.user.userEmployeeId).subscribe( x => {
        this.employeeCostCenterId = x.costCenterId ? x.costCenterId : 0;
      });
      this.api.clockTimeCardItems$.pipe(skip(1)).subscribe((data: ClockTimeCard[]) => {
        this.firstDate = data[0].date;
        this.matList   = new MatTableDataSource<ClockTimeCard>(data);
        this.isLoading = false;
      });

      this.api.fetchTimeCard(this.user.clientId, this.user.userEmployeeId);
    });

  }

  openModal(url: string) {
    let w = window,
    d = document,
    e = d.documentElement,
    g = d.getElementsByTagName('body')[0],
    x = 500,
    y = 550,
    xt = w.innerWidth || e.clientWidth || g.clientWidth,
    yt = w.innerHeight|| e.clientHeight|| g.clientHeight;

    var left = (xt - x) / 2;
    var top = (yt - y) / 4;

    !!this.popup && this.popup.unsubscribe();
    this.popup = this.accountService.getSiteConfig(ConfigUrlType.Payroll)
      .pipe(switchMap(config => {
        url = config.url.replace(/\/$|$/, '/') + url;
        return this.DsPopup
          .open(url, '_blank', { height: y, width: x, top: top, left: left })
          .closed();
      }))
      .subscribe(() => this.api.fetchTimeCard(this.user.clientId, this.user.userEmployeeId));
  }

}
