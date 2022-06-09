import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '../shared/employee.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ActivatedRoute } from '@angular/router';
import { ClockTimeCard } from '../shared/models/clock-client-time-card.model';
import { MatTableDataSource } from '@angular/material/table';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { skip, tap } from 'rxjs/operators';

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

  constructor(
    private api : EmployeeService,
    public accountService : AccountService,
    private msg : DsMsgService,
    private activatedRoute: ActivatedRoute,
    public clockService: ClockService,
    private DsPopup: DsPopupService

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

  openModal(url: String) {
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

    var modal = this.DsPopup.open(url, '_blank', { height: y, width: x, top: top, left: left });
    modal.closed().then(() => { this.api.fetchTimeCard(this.user.clientId, this.user.userEmployeeId); });
  }

}
