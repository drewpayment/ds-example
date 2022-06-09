import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { MatDialog } from "@angular/material/dialog";
import { ClockedInEmployees } from '@ds/analytics/shared/models/EmployeesClockedInData.model';
import { EmployeesClockedInDialogComponent } from './employees-clocked-in-dialog/employees-clocked-in-dialog.component';
import { AccountService } from '@ds/core/account.service';
import { PayrollService } from '@ds/payroll/shared/payroll.service';


@Component({
    selector: "ds-employees-clocked-in",
    templateUrl: "./employees-clocked-in.component.html",
    styleUrls: ["./employees-clocked-in.component.css"],
})
export class EmployeesClockedInComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    loaded: boolean;
    emptyState = false;
    infoData: InfoData;
    clockedInEmployees: ClockedInEmployees[] = [];
    overtime: number = 0;
    cardType: string = 'info';
    obj = [];
    periodStart;
    periodEnded;

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog,
        private payrollApi: PayrollService,
    ) {}

    ngOnInit() {
      let today = new Date();
      let startDate = new Date();
      startDate.setDate(today.getDate()-7);
      this.accountService.getUserInfo().subscribe((user) => { this.payrollApi.getCurrentPayrollByClientId(user.clientId).subscribe((res: any) => {
        this.analyticsApi.GetClockedInEmployees(user.clientId, this.employeeIds).subscribe((employees: any) => {
          this.analyticsApi.GetOvertimeSetup(user.clientId, startDate, today, this.employeeIds).subscribe((overtime: any) => {
            this.clockedInEmployees = employees;
            var ot = overtime.data;
            if (this.clockedInEmployees != null){
              this.combineData(this.clockedInEmployees, ot)
            }
              this.infoData = {
                  icon: "access_time",
                  color: "primary",
                  value: this.lengthReturned(this.clockedInEmployees),
                  title: `EMPLOYEES CURRENTLY CLOCKED IN`,
                  showBottom: false,
              };
              this.loaded = true;
            });
          });
        });
      });
    }

    combineData(employeeData, otData){
      var data = [];
      var hour = new Date().getHours();
      var minute = new Date().getMinutes();
      let info = this.clockedInEmployees.map(x => x.employeeID)
      var otEEs = [];
      for (var i = 0; i < otData.length; i++){
        for (var j = 0; j < info.length; j++){
          if (otData[i].employeeId === info[j]){
            otEEs.push(otData[i].hours)
          }
        }
      }
      for (var x = 0; x < employeeData.length; x++){
        if (otData.length === 0){
          data.push({
            employeeName: employeeData[x].employeeName.trim(),
            supervisorName: this.assignSupervisor(employeeData[x].filter),
            department: employeeData[x].department,
            hoursScheduled: this.getSchedule(employeeData[x].startTime, employeeData[x].stopTime),
            hoursWorked: this.getTime(hour, minute, employeeData[x].lastPunch.substring(11)),
            overtime: 0
          })
        }
        else{
          data.push({
            employeeName: employeeData[x].employeeName.trim(),
            supervisorName: this.assignSupervisor(employeeData[x].filter),
            department: employeeData[x].department,
            hoursScheduled: this.getSchedule(employeeData[x].startTime, employeeData[x].stopTime),
            hoursWorked: this.getTime(hour, minute, employeeData[x].lastPunch.substring(11)),
            overtime: otEEs[x]
          })
        }
      }
      this.obj = data;
    }

    assignSupervisor(name){
      if (name == ", ") return "";
      if (name == "") return "Unassigned";
      return name;
    }

    getSchedule(start, end){
      if (start == null || end == null) return 0;
      var starting = Date.parse(start);
      var ending = Date.parse(end);
      return ((ending / (1000 * 60 * 60)) % 24) - ((starting/ (1000 * 60 * 60)) % 24);
    }

    getTime(hours, minutes, timeStamp){
      var amOrPm = timeStamp.substring(timeStamp.indexOf("M")-1);
      var hour = parseInt(timeStamp.substring(0,timeStamp.indexOf(":")));
      var minute = parseInt(timeStamp.substring(timeStamp.indexOf(":")+1));
      if (hour < 12 && amOrPm === "PM") hour = hour + 12;
      hour = hours - hour;
      minute = minutes - minute;
      if (minute < 0){
        hour = hour - 1;
        minute = 60 + minute;
      }
      if (minute < 10) return `${hour}:0${minute}`;
      return `${hour}:${minute}`
    }

    lengthReturned(data){
      if (data != null)
      return `${data.length}`;
      else{
        return "0";
      }
    }

    openDialog(){
      if(this.loaded){}
      var config = {
        width: '1000px',
        data: {
          employeeData: this.obj,
          title: `Employees that are currently clocked in: ${this.lengthReturned(this.clockedInEmployees)}`,
          dateRange: this.dateRange
        }
      };

      const dialogRef = this.dialog.open(EmployeesClockedInDialogComponent, config);
    }

}
