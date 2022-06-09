import { Component, OnInit, Input } from '@angular/core';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { TerminationData } from '@ds/analytics/shared/models/TerminationData.model';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import { MatDialog } from '@angular/material/dialog';
import * as moment from 'moment';
import { EmployeeGrowthRateDialogComponent } from './employee-growth-rate-dialog/employee-growth-rate-dialog.component';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-employee-growth-rate',
  templateUrl: './employee-growth-rate.component.html',
  styleUrls: ['./employee-growth-rate.component.css']
})
export class EmployeeGrowthRateComponent implements OnInit {

  @Input() dateRange: DateRange;
  @Input() employeeIds: Number[];
  @Input() activeFilters: string[];

  cardType = "info";
  growthData: TerminationData;
  previousYear: TerminationData;
  newHires: TerminationData;
  loaded: boolean = false;

  infoData: InfoData;

  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi.GetGrowthRate(user.clientId, moment(new Date(this.dateRange.StartDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((previousYear: any) => {
        this.analyticsApi.GetGrowthRate(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((growthData: any) => {
          this.analyticsApi.GetNewHiredEmployeesDetailFn(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((newHireEE: any) => {
            this.previousYear = previousYear.data;
            this.growthData = growthData.data;
            this.newHires = newHireEE.data;

            var previousGrowth = ((this.previousYear.endCount - this.previousYear.startCount)/(this.previousYear.startCount)) * 100;
            var currentGrowth = ((this.growthData.endCount - this.growthData.startCount)/(this.growthData.startCount)) * 100;

            if(new Date(this.dateRange.StartDate).getFullYear() != 1700){
              this.previousYear.growthRate = `${previousGrowth.toFixed(2)}%`;
            } else {
              this.previousYear.growthRate = '---';
            }

            if(this.growthData.growthRate == '-2147483650.00' || this.growthData.startCount == 0 ){
              this.growthData.growthRate = 'N/A';
              currentGrowth = 0;
            }

            if(this.previousYear.growthRate == '-2147483650.00' || this.previousYear.startCount == 0){
              this.previousYear.growthRate = 'N/A';
              previousGrowth = 0;
            }

            this.infoData = {
              icon: 'timeline',
              color: 'success',
              value: !currentGrowth ? `N/A` : `${currentGrowth.toFixed(2)}%`,
              title: 'Growth',
              tooltip: 'The change in employee headcount from the start of a time period to the end of that time period.',
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
          growthData: this.newHires,
          dateRange: this.dateRange,
          filters: this.activeFilters,
          title: 'Overall Growth Rate',
          instructionalText: 'Overall growth rate is the percentage of employees who are hired during a certain period of time. This is calculated by subtracting the number of employees at the start of a time period by the number of employees at the end of a time period, then divided by the number of employees at the start of the time period.'
        }
      };

      const dialogRef = this.dialog.open(EmployeeGrowthRateDialogComponent, config);
    }
  }
}
