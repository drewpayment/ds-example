import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { EmployeeService } from 'apps/ds-source/src/app/employee/shared/employee.service';
import { UserInfo } from '@ds/core/shared';
import { AccountService } from '@ds/core/account.service';
import { DsMsgService } from '@ajs/core/msg/ds-msg.service';
import { ActivatedRoute } from '@angular/router';
import { ClockTimeCard } from 'apps/ds-source/src/app/employee/shared/models/clock-client-time-card.model';
import { MatTableDataSource } from '@angular/material/table';
import { tap } from 'rxjs/internal/operators/tap';
import { ClockService } from '@ds/core/employee-services/clock.service';
import { Observable, Subject, BehaviorSubject, iif, of } from 'rxjs';
import { DsPopupService } from '@ajs/ui/popup/ds-popup.service';
import { skip, switchMap } from 'rxjs/operators';
import { coerceBooleanProperty } from '@angular/cdk/coercion';
import { resolve } from 'url';
import { TimecardParams } from '@ds/core/employees/shared/timecard-params.model'
import * as moment from 'moment';

@Component({
  selector: 'ds-employee-clock-timecard-widget',
  templateUrl: './employee-clock-timecard-widget.component.html',
  styleUrls: ['./employee-clock-timecard-widget.component.scss']
})
export class EmployeeClockTimecardWidgetComponent implements OnInit {
  @Input() employeeId:  number;

  clientId: number;
  user: UserInfo;
  matList: any;
  rows: ClockTimeCard[];
  displayedColumns: string[] = ['date', 'punches', 'hours','schedule','exceptions','notes'];
  firstDate: String;
  isLoading = true;
  employeeCostCenterId: number;
  params:TimecardParams;
  timeCardHeading: string;

  constructor(
    private api : EmployeeService,
    public accountService : AccountService,
    private msg : DsMsgService,
    private activatedRoute: ActivatedRoute,
    public clockService: ClockService,
    private DsPopup: DsPopupService

  ) { }

  ngOnInit() {
    // when there is a change in filter selection
    this.checkCurrentUser().pipe(
    	switchMap(user => this.clockService.getPeriodFilterParams())).subscribe( x => {

      if(x){
        this.isLoading = true;
        this.params = x;

        this.refreshGrid();
      }
      else
        this.params = { payrollId: -1, isCustom: true, periodStart: new Date() , periodEnd: new Date() };
    });

    // getUserInformation$
    this.checkCurrentUser().subscribe(() => {
      this.api.getEmployeeWorkInfo(this.employeeId).subscribe( x => {
        this.employeeCostCenterId = (x && x.costCenterId) ? x.costCenterId : 0;
      });
      this.api.clockTimeCardItems$.pipe(skip(1)).subscribe((data: ClockTimeCard[]) => {
        this.firstDate = data.length > 0 ? data[0].date : (new Date()).toLocaleDateString();
        this.matList   = new MatTableDataSource<ClockTimeCard>(data);
        this.isLoading = false;
      });
    });

  }

  refreshGrid(){
    if( !this.params.isCustom ){
      if(this.params.payrollId == 0){
        this.timeCardHeading = "Current Pay Period";
        this.api.fetchTimeCard(this.clientId, this.employeeId, this.params.payrollId, this.params.periodStart, this.params.periodEnd );
      } else {
        this.api.fetchTimeCard(this.clientId, this.employeeId, this.params.payrollId );
        this.timeCardHeading = "Pay Period " + this.dateRangeStr();
      }
    } else {
      this.api.fetchTimeCard(this.clientId, this.employeeId, -1, this.params.periodStart, this.params.periodEnd );
      this.timeCardHeading = "Custom Date Range " + this.dateRangeStr();
    }
  }

  checkCurrentUser(): Observable<UserInfo>{
    return iif(() => this.user == null,
        this.accountService.getUserInfo().pipe(tap(u => {
            this.user = u;
            if(!this.employeeId)  this.employeeId = this.user.userEmployeeId;
            this.clientId     = this.user.clientId;
        })),
        of(this.user));
  }

  initialDate(){
    var fromDate = new Date();
    var curDayStart = fromDate.getDay();
    fromDate.setDate(fromDate.getDate() - curDayStart)
    return fromDate.toLocaleDateString();
  }

  dateRangeStr(){
    return moment(this.params.periodStart).format('MM/DD/YYYY') + ' - ' + moment(this.params.periodEnd).format('MM/DD/YYYY');
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
    modal.closed().then(() => { this.refreshGrid() });
  }

}
