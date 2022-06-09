import { Component, OnInit, Input } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { AccountService } from '@ds/core/account.service';
import { MatDialog } from '@angular/material/dialog';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { OvertimeDialogComponent } from './overtime-dialog/overtime-dialog.component';
import { PayrollService } from '@ds/payroll/shared/payroll.service';
import * as moment from "moment";
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-overtime',
  templateUrl: './overtime.component.html',
  styleUrls: ['./overtime.component.css']
})
export class OvertimeComponent implements OnInit {

  @Input() employeeIds: number[];
  @Input() dateRange: DateRange;

  cardType = "info";
  infoData: InfoData;
  loaded: boolean = false;
  emptyState: boolean = false;
  employeeSchedules: any[] = [];
  employeeData: any[];
  otData: any[];
  otEmployeeIDs: number[];


  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog, private payrollApi: PayrollService,) { }

  ngOnInit() {
    let endDate = moment(new Date(this.dateRange.EndDate));
    let startDate = moment(new Date(this.dateRange.StartDate));

    let filterType = '';
    if (this.dateRange.type)  filterType = this.dateRange.type.toLowerCase();
    
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi.GetOvertimeSetup(user.clientId, startDate.format(MOMENT_FORMATS.DATE), endDate.format(MOMENT_FORMATS.DATE), this.employeeIds).subscribe((res: any) => {
      this.otData = res.data;
      this.otEmployeeIDs = res.data.map(x => x.employeeId);

      this.analyticsApi.GetGetClockEmployeeHoursComparison(user.clientId, startDate.format(MOMENT_FORMATS.DATE), endDate.format(MOMENT_FORMATS.DATE), this.employeeIds,).subscribe((data: any) => {
        if (data == null || data == [] || data.length <=0){
          this.emptyState = true;
          this.loaded = true;
        }
        else{
          this.employeeData = data;
          this.setSchedules(this.employeeData);
        }
      });
      this.setData(this.otData);
      this.loaded = true
      })
    });
  }


  setSchedules(arr){
    let unique = this.uniqueBy(arr, "employeeId");
    unique.forEach(eID => {
      let scheduled = 0;
      let worked = 0;
      let loop = this.employeeData.filter(x => x.employeeId == eID);
      loop.forEach(element => {
        scheduled += element.hoursScheduled;
        worked += element.actualHours;
      });
      this.employeeSchedules.push({
        employeeId: eID,
        scheduledHours: scheduled,
        workedHours: worked
      })
    });

    this.otData.forEach(employee => {
      this.employeeSchedules.forEach(employeeSchedule => {
        if (employee.employeeId == employeeSchedule.employeeId){
          employee.hoursScheduled = employeeSchedule.scheduledHours;
          employee.hoursWorked = employeeSchedule.workedHours;
        }
      });
    })
  }

  dateRangeLabel():string{
    let endDate = moment(new Date(this.dateRange.EndDate));
    let startDate = moment(new Date(this.dateRange.StartDate));

    let filterType = '';
    if (this.dateRange.type)  filterType = this.dateRange.type.toLowerCase();

    if(startDate.date() == endDate.date() && startDate.month() == endDate.month() && startDate.year() == endDate.year() )
      return "\nOn "+ endDate.format(MOMENT_FORMATS.US)
    else if (filterType == 'current week') {
      return "\nWeek of " + startDate.format(MOMENT_FORMATS.US);
    } else {
      return "\nFrom " + startDate.format(MOMENT_FORMATS.US) + " to " + endDate.format(MOMENT_FORMATS.US);
    }
  }

  setData(otEmployees) {
    let lbl:string = this.dateRangeLabel();
    this.infoData = {
      icon: "av_timer",
      color: "warning",
      value: otEmployees.length.toString(),
      title: `EMPLOYEES WORKING OVERTIME ${lbl}`,
      showBottom: false
    };
  }

  openDialog() {
        var config = {
          width: '1000px',
          data: {
            overtimeWorkers: this.otData
          }
        };
        const dialogRef = this.dialog.open(OvertimeDialogComponent, config);
  }

  uniqueBy(arr, prop){
    return arr.reduce((a, d) => {
        if (!a.includes(d[prop])) { a.push(d[prop]); }
        return a;
    }, []);
  }
}
