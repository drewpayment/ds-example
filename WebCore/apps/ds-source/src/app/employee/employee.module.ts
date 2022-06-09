import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  Routes,
  RouterModule,
} from "@angular/router";
import { EmployeeOutletComponent } from "./employee-outlet/employee-outlet.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { EmployeeDeductionOutletComponent } from "./employee-deduction-outlet/employee-deduction-outlet.component";
import { MaterialModule } from "@ds/core/ui/material";
import { DsCardModule } from "@ds/core/ui/ds-card";
import { ClockTimeCardWidgetComponent } from "./clock-time-card-widget/clock-time-card-widget.component";
import { InputPunchesWidgetComponent } from "./input-punches-widget/input-punches-widget.component";
import { NoPaycheckModalComponent } from "./no-paycheck-modal/no-paycheck-modal.component";
import { PunchService } from "@ds/core/timeclock/punch.service";
import { EmployeeDeductionsComponent, EmployeeModule } from "@ds/employee";
import { EmployeeExitInterviewRequestComponent } from "./employee-exit-interview-request/employee-exit-interview-request.component";
import { DsCoreResourcesModule } from "@ds/core/resources";
import { ElectronicConsentsModule } from "@ds/core/employees/electronic-consents/electronic-consents.module";
import { ElectronicConsentsComponent } from "@ds/core/employees/electronic-consents/electronic-consents.component";
import { EmployeeEEOCExportComponent } from "./employee-eeoc/employee-eeoc-export.component";
import { EEOCCreateReportComponent } from "./employee-eeoc/eeoc-create-report/eeoc-create-report.component";
import { EEOCCompanySelectComponent } from "./employee-eeoc/eeoc-company-select/eeoc-company-select.component";
import { EEOCValidateLocationsComponent } from "./employee-eeoc/eeoc-validate-locations/eeoc-validate-locations.component";
import { EEOCValidateEmployeesComponent } from "./employee-eeoc/eeoc-validate-employees/eeoc-validate-employees.component";
import { LocationFilterPipe } from "./employee-eeoc/eeoc-validate-employees/location-filter-pipe";
import { PayrollFilterPipe } from "./employee-eeoc/eeoc-company-select/payroll-filter-pipe";
import { EEOCLocationsModalComponent } from "./employee-eeoc/eeoc-locations-modal/eeoc-locations-modal.component";
import { EEOCLocationsTriggerComponent } from "./employee-eeoc/eeoc-locations-modal/eeoc-locations-trigger.component";
import { LoadingMessageModule } from "@ds/core/ui/loading-message/loading-message.module";
import { NgxMaskModule } from "ngx-mask";
import { options } from "@ds/core/ui/forms";
import { NpsModule } from "@ds/admin/nps/nps.module";
import { EmployeePayComponent } from "./employee-pay/employee-pay.component";
import { PayrollModule } from "@ds/payroll/payroll.module";
import { EmployeeDeductionsGuard } from "./employee-deduction-outlet/employee-deductions.guard";
import { MatInputModule } from "@angular/material/input";
import { MatTooltipModule } from "@angular/material/tooltip";
import { AvatarModule } from "@ds/core/ui/avatar/avatar.module";

export const employeeRoutes: Routes = [
  {
    path: "employee",
    children: [
      {
        path: "deductions",
        component: EmployeeDeductionsComponent,
        canActivate: [EmployeeDeductionsGuard],
      },
      {
        path: "consents",
        component: ElectronicConsentsComponent,
        //canActivate: [],
      },
    ],
  },
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatInputModule,
    DsCardModule,
    EmployeeModule,
    DsCoreResourcesModule,
    ElectronicConsentsModule,
    LoadingMessageModule,
    MatTooltipModule,
    NgxMaskModule.forRoot(options),
    NpsModule,
    PayrollModule,
    AvatarModule,
    RouterModule.forChild(employeeRoutes),
  ],
  declarations: [
    EmployeeOutletComponent,
    ClockTimeCardWidgetComponent,
    InputPunchesWidgetComponent,
    NoPaycheckModalComponent,
    EmployeeDeductionOutletComponent,
    EmployeeExitInterviewRequestComponent,
    EmployeeEEOCExportComponent,
    EEOCCreateReportComponent,
    EEOCCompanySelectComponent,
    EEOCValidateLocationsComponent,
    EEOCValidateEmployeesComponent,
    LocationFilterPipe,
    PayrollFilterPipe,
    EEOCLocationsModalComponent,
    EEOCLocationsTriggerComponent,
    EmployeePayComponent,
  ],
  entryComponents: [
    NoPaycheckModalComponent,
    EmployeeExitInterviewRequestComponent,
    EEOCLocationsModalComponent,
    EEOCLocationsTriggerComponent,
    ElectronicConsentsComponent,
  ],
  providers: [PunchService, LocationFilterPipe, EmployeeDeductionsGuard],
})
export class EmployeeAppModule {}
