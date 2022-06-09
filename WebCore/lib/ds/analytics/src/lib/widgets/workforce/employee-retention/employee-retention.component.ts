import { Component, OnInit, Input } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { TerminationData, EmployeeTermination } from '@ds/analytics/shared/models/TerminationData.model';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import * as moment from 'moment';
import { MatDialog } from '@angular/material/dialog';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';
import { ActiveEmployee } from '@ds/analytics/shared/models/ActiveEmployeeData.model';
import { EmployeeGrowthRateDialogComponent } from '../employee-growth-rate/employee-growth-rate-dialog/employee-growth-rate-dialog.component';

@Component({
  selector: 'ds-employee-retention',
  templateUrl: './employee-retention.component.html',
  styleUrls: ['./employee-retention.component.css']
})
export class EmployeeRetentionComponent implements OnInit {

  @Input() dateRange: DateRange;
  @Input() employeeIds: Number[];
  @Input() activeFilters: string[];

  cardType = "info";
  retentionData: TerminationData;
  previousYear: TerminationData;
  retainedEEs: EmployeeTermination[];
  loaded: boolean = false;

  infoData: InfoData;

  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi.GetRetentionRate(user.clientId, moment(new Date(this.dateRange.StartDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((previousYear: any) => {
        this.analyticsApi.GetRetentionRate(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((turnoverData: any) => {
          this.analyticsApi.GetActiveEmployees(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((retainedEE: any) => {
            this.previousYear = previousYear.data;
            this.retentionData = turnoverData.data;
            this.retainedEEs = [];
            let k:ActiveEmployee[] = retainedEE.filter(x => moment(x.hireDate).toDate() < moment(this.dateRange.StartDate).toDate());
            this.retainedEEs = k.map(x => <EmployeeTermination>{
                                      active: true,
                                      costCenter: x.clientCostCenterName,
                                      department: x.clientDepartmentName,
                                      employeeId: x.employeeId,
                                      employeeStatus: x.employeeStatus,
                                      fullName: x.lastName +', '+x.firstName,
                                      hireDate: x.hireDate,
                                      lastName: x.lastName,
                                      separationDate: x.separationDate,
                                      terminationDate: null,
                                      terminationReason: '' });

            var previousRetention = ((this.previousYear.startCount - this.previousYear.termedCount) / (this.previousYear.startCount)) * 100
            var currentRetention = ((this.retentionData.startCount - this.retentionData.termedCount) / (this.retentionData.startCount)) * 100

            if (this.retentionData.termedCount === 0) currentRetention = 100;

            if(new Date(this.dateRange.StartDate).getFullYear() != 1700){
              this.previousYear.retentionRate = `${previousRetention.toFixed(2)}%`;
            } else {
              this.previousYear.retentionRate = '---';
            }

            if(this.retentionData.retentionRate == '-2147483650.00' || this.retentionData.startCount == 0 ){
              this.retentionData.retentionRate = 'N/A';
              currentRetention = 0;
            }

            if(this.previousYear.retentionRate == '-2147483650.00' || this.previousYear.startCount == 0){
              this.previousYear.retentionRate = 'N/A';
              previousRetention = 0;
            }

            console.log(JSON.stringify(
              { current: {s:this.retentionData.startCount,e:this.retentionData.endCount,t:this.retentionData.termedCount,n:this.retentionData.newCount,i:this.retentionData.hiredAndTermedCount},
              prev: {s:this.previousYear.startCount,e:this.previousYear.endCount,t:this.previousYear.termedCount,n:this.previousYear.newCount,i:this.previousYear.hiredAndTermedCount} }
              ));

            this.infoData = {
              icon: 'business',
              color: 'success',
              value: !currentRetention ? `N/A` : `${currentRetention.toFixed(2)}%`,
              title: 'Retention',
              tooltip: 'The number of employees hired within a time period who are still active at the end of that time period.',
              showBottom: true
            };

            this.loaded = true;
          })
        })
      })
    })
  }

  openDialog(){
    if(this.loaded && this.employeeIds && this.employeeIds.length > 0){
      var config = {
        width: '1000px',
        data: {
          growthData: this.retainedEEs,
          dateRange: this.dateRange,
          filters: this.activeFilters,
          title: 'Overall Retention',
          instructionalText: 'Overall retention is the percentage of employees who stay at a company during a certain period of time. This is calculated by dividing the number of active employees during a period of time by the number of employees at the start of that time period.'
        }
      };

      const dialogRef = this.dialog.open(EmployeeGrowthRateDialogComponent, config);
    }
  }

}
