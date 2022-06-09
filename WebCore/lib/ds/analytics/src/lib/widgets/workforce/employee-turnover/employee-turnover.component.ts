import { Component, OnInit, Input } from '@angular/core';
import { InfoData } from '@ds/analytics/shared/models/InfoData.model';
import { DateRange } from '@ds/analytics/shared/models/DateRange.model';
import { AnalyticsApiService } from '@ds/analytics/shared/services/analytics-api.service';
import * as moment from 'moment';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeTurnoverDialogComponent } from './employee-turnover-dialog/employee-turnover-dialog.component';
import { TerminationData } from '@ds/analytics/shared/models/TerminationData.model';
import { AccountService } from '@ds/core/account.service';
import { MOMENT_FORMATS } from '@ds/core/shared';

@Component({
  selector: 'ds-employee-turnover',
  templateUrl: './employee-turnover.component.html',
  styleUrls: ['./employee-turnover.component.css']
})
export class EmployeeTurnoverComponent implements OnInit {

  @Input() dateRange: DateRange;
  @Input() employeeIds: Number[];
  @Input() activeFilters: string[];

  cardType = "info";
  turnoverData: TerminationData;
  previousYear: TerminationData;
  terminatedEEs: TerminationData;
  loaded: boolean = false;

  infoData: InfoData;

  constructor(private analyticsApi: AnalyticsApiService, private accountService: AccountService, private dialog: MatDialog) { }

  ngOnInit() {
    this.accountService.getUserInfo().subscribe((user) => {
      this.analyticsApi.GetTurnoverRate(user.clientId, moment(new Date(this.dateRange.StartDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).subtract(1, 'years').format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((previousYear: any) => {
        this.analyticsApi.GetTurnoverRate(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((turnoverData: any) => {
          this.analyticsApi.GetTerminatedEmployees(user.clientId, moment(new Date(this.dateRange.StartDate)).format(MOMENT_FORMATS.DATE), moment(new Date(this.dateRange.EndDate)).format(MOMENT_FORMATS.DATE), this.employeeIds, user.userId, user.userTypeId).subscribe((terminatedEE: any) => {
            this.previousYear = previousYear.data;
            this.turnoverData = turnoverData.data;
            this.terminatedEEs = terminatedEE.data;

            var previousTurnover = ((this.previousYear.termedCount)/((this.previousYear.startCount + this.previousYear.endCount)/2)) * 100;
            var currentTurnover = ((this.turnoverData.termedCount)/((this.turnoverData.startCount + this.turnoverData.endCount)/2)) * 100;

            if(new Date(this.dateRange.StartDate).getFullYear() != 1700){
              this.previousYear.turnoverRate = `${previousTurnover.toFixed(2)}%`;
            } else {
              this.previousYear.turnoverRate = '---';
            }
	    
            if(this.turnoverData.retentionRate == '-2147483650.00' || (this.turnoverData.startCount + this.turnoverData.endCount) == 0 ){
              this.turnoverData.turnoverRate = 'N/A';
              currentTurnover = 0;
            }

            if(this.previousYear.turnoverRate == '-2147483650.00' || (this.previousYear.startCount + this.previousYear.endCount) == 0){
              this.previousYear.turnoverRate = 'N/A';
              previousTurnover = 0;
            }

            this.infoData = {
              icon: 'autorenew',
              color: 'success',
              value: !currentTurnover ? `N/A` : `${currentTurnover.toFixed(2)}%`,
              title: 'Turnover',
              tooltip: 'The number of employees that left during a time period divided by the average number of active employees for that time period.',
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
          turnoverData: this.terminatedEEs,
          dateRange: this.dateRange,
          filters: this.activeFilters,
          title: 'Overall Turnover',
          instructionalText: 'Overall turnover is the percentage of employees who leave a company during a certain period of time. This is calculated by dividing the number of employees that left by the average number of active employees for that time period.'
        }
      };

      const dialogRef = this.dialog.open(EmployeeTurnoverDialogComponent, config);
    }
  }

}
