import { Component, OnInit, Inject } from "@angular/core";
import {
  MatDialogRef,
  MatDialog,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { EmployeeApiService } from "@ds/core/employees";
import { DsMsgService } from "@ajs/core/msg/ds-msg.service";
// import { IAccountService }                                                                    from "@ds/core/account/account.interface";
// import { FormUtilsService }                                                                   from "@ds/ui/forms/utils/form-utils.service";
// import { IDsMessageService }                                                                  from "@ds/core/msg/ds-msg.interface";
import { IContact } from "@ds/core/contacts/shared/contact.model";
import { IEmployeeDirectSupervisorLink } from "@ds/performance/performance-manager/shared/employee-direct-supervisor-link.model";
import { HttpErrorResponse } from "@angular/common/http";
import { PerformanceManagerService } from "@ds/performance/performance-manager/performance-manager.service";
import { map } from "rxjs/operators";

interface DialogData {
  employeeId: number;
  name: string;
}

@Component({
  selector: "ds-supervisor-outlet",
  templateUrl: "./supervisor-outlet.component.html",
  styleUrls: ["./supervisor-outlet.component.scss"],
})
export class SupervisorOutletComponent implements OnInit {
  supervisors: IContact[];
  selectedSupervisor: IContact;
  linkDto: IEmployeeDirectSupervisorLink;

  constructor(
    public dialogRef: MatDialogRef<SupervisorOutletComponent>,
    private employeeService: EmployeeApiService,
    private performanceService: PerformanceManagerService,
    private msgSvc: DsMsgService,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  ngOnInit() {
    this.performanceService
      .getDirectSupervisors()
      .pipe(
        map((x) => {
          return x.sort((contactA, contactB) => {
            return contactA.lastName.localeCompare(contactB.lastName);
          });
        })
      )
      .subscribe((supers) => {
        this.supervisors = supers;
      });
  } // end of ngOnInit()

  save() {
    this.linkDto = { employeeId: 0, directSupervisorId: 0 };
    this.linkDto.employeeId = this.data.employeeId;
    this.linkDto.directSupervisorId = this.selectedSupervisor.userId;
    this.performanceService.saveEmloyeeDirectSupervisor(this.linkDto).subscribe(
      (data) => {
        this.msgSvc.setTemporarySuccessMessage(
          "Successfully assigned the direct supervisor to the employee."
        );
        const supervisor = this.supervisors.find(
          (s) => s.userId == data.directSupervisorId
        );
        this.dialogRef.close(supervisor);
      },
      (error: HttpErrorResponse) => {
        this.msgSvc.showWebApiException(error.error);
      }
    );
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
} // end of class
