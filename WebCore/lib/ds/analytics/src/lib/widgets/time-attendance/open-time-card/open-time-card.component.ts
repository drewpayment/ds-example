import { Component, OnInit, Input } from "@angular/core";
import { DateRange } from "@ds/analytics/shared/models/DateRange.model";
import { InfoData } from "@ds/analytics/shared/models/InfoData.model";
import { AnalyticsApiService } from "@ds/analytics/shared/services/analytics-api.service";
import { MatDialog } from '@angular/material/dialog';
import { OpenTimeCardDialogComponent } from './open-time-card-dialog/open-time-card-dialog.component';
import { AccountService } from '@ds/core/account.service';
import { TimeCards } from '@ds/analytics/shared/models/OpenTimeCards.model';
import { PayrollService } from '@ds/payroll/shared/payroll.service';

@Component({
    selector: "ds-open-time-card",
    templateUrl: "./open-time-card.component.html",
    styleUrls: ["./open-time-card.component.css"],
})
export class OpenTimeCardComponent implements OnInit {
    @Input() employeeIds: Number[];
    @Input() dateRange: DateRange;

    cardType = "info";
    loaded: boolean = false;
    timeCards: TimeCards[] = [];
    unapproved: TimeCards[] = [];

    infoData: InfoData;

    constructor(
        private analyticsApi: AnalyticsApiService,
        private accountService: AccountService,
        private dialog: MatDialog,
        private payrollApi: PayrollService
    ) {}

    ngOnInit() {
        this.accountService.getUserInfo().subscribe((user) => {
          this.payrollApi.getCurrentPayrollByClientId(user.clientId).subscribe((res: any) => {
            this.analyticsApi.GetTimeCardApproval(user.clientId, res.payrollId, res.periodStart, res.periodEnded, user.userId).subscribe((timeCards: any) => {
              if (timeCards.supervisors.length <= 0 && timeCards.unassignedEmployeeStatuses.length <= 0){
                  this.infoData = {
                    icon: "content_paste",
                    color: "danger",
                    value: "0",
                    title: "SUPERVISORS NEED TO APPROVE TIME CARDS",
                    showBottom: false,
                };
              }
              else{
                this.timeCards = timeCards;
                this.combo(this.timeCards);
                this.infoData = {
                    icon: "content_paste",
                    color: "danger",
                    value: this.unapproved.length.toString(),
                    title: "SUPERVISORS NEED TO APPROVE TIME CARDS",
                    showBottom: false,
                };
              }
              this.loaded = true;
          });
        });
      });
    }

    combo(supervisorData){
      var obj = [];
      var assignedSupervisor = [];
      var unassignedSupervisor = [];
      var unapprovedEvents = [];
      var employeeId = [];
      var approval = [];
      var approvedEEs = [];
      var unapprovedEEs = [];
      var unassignedApproved = [];
      var unassignedUnapproved = [];

      var result = Object.keys(supervisorData).map((key) => [supervisorData[key]]);
      assignedSupervisor = result[0][0];
      unassignedSupervisor = result[1];
      unapprovedEvents = result[2];

      for (var i = 0; i < assignedSupervisor.length; i++){
        assignedSupervisor[i].isApproved = true;
      }

      employeeId = unapprovedEvents[0].map(x => x.employeeID);

      for (var i = 0; i < assignedSupervisor.length; i++){
        approval[i] = assignedSupervisor[i].employeeApprovals;
      }

      for(var i = 0; i < approval.length; i++){
        for (var j = 0; j < employeeId.length; j++){
          for (var k = 0; k < approval[i].length; k++){
            if (approval[i][k].employeeId === employeeId[j]){
              assignedSupervisor[i].isApproved = false;
            }
          }
        }
      }

      unapprovedEEs = assignedSupervisor.filter(x => x.isApproved === false);
      approvedEEs = assignedSupervisor.filter(x => x.isApproved === true);

      unapprovedEEs.sort((a, b) => (a.lastName > b.lastName) ? 1: ((b.lastName > a.lastName) ? -1 : 0));
      approvedEEs.sort((a, b) => (a.lastName > b.lastName) ? 1: ((b.lastName > a.lastName) ? -1 : 0));

      if (unassignedSupervisor[0].length > 0){
        unassignedApproved = unassignedSupervisor[0].filter(x => x.isApproved === true);
        unassignedUnapproved = unassignedSupervisor[0].filter(x => x.isApproved === false);
      }

      if(unassignedUnapproved.length > 0 && (unassignedApproved.length === 0 || unassignedApproved.length > 0)){
        unapprovedEEs.push({
          name: "Unassigned",
          approved: "Unapproved"
        })
      }

      if(unassignedUnapproved.length === 0 && unassignedApproved.length > 0){
        approvedEEs.push({
          name: "Unassigned",
          approved: "Approved"
        })
      }

      this.unapproved = unapprovedEEs;
      for (var i = 0; i < unapprovedEEs.length; i++){
        if (unapprovedEEs[i].name === "Unassigned") {
          obj.push(unapprovedEEs[i]);
          continue;
        }
        obj.push({
          name: `${unapprovedEEs[i].lastName}, ${unapprovedEEs[i].firstName}`,
          approved: "Unapproved"
        });
      }
      for (var i = 0; i < approvedEEs.length; i++){
        if (approvedEEs[i].name === "Unassigned") {
          obj.push(approvedEEs[i]);
          continue;
        }
        obj.push({
          name: `${approvedEEs[i].lastName}, ${approvedEEs[i].firstName}`,
          approved: "Approved"
        });
      }
      this.timeCards = obj;
    }

    openDialog(){
      if(this.loaded){
        var config = {
          width: '1000px',
          data: {
            timeCards: this.timeCards,
            title: 'Supervisors Time Cards',
            instructionalText: 'These are the unapproved and approved supervisor time cards',
            dateRange: this.dateRange
          }
        };

        const dialogRef = this.dialog.open(OpenTimeCardDialogComponent, config);
      }
    }
}
