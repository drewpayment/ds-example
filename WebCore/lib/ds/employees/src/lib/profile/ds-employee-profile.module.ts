import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { EmployeeBioComponent } from './employee-bio/employee-bio.component';
import { EmployeeBioFormComponent } from './employee-bio-form/employee-bio-form.component';
import { EmployeeDependentsComponent } from './employee-dependents/employee-dependents.component';
import { EmployeeDependentsFormComponent } from './employee-dependents-form/employee-dependents-form.component';
import { EmergencyContactListComponent } from './emergency-contacts/emergency-contact-list/emergency-contact-list.component';
import { EmergencyContactFormComponent } from './emergency-contacts/emergency-contact-form/emergency-contact-form.component';
import { EmployeeContactInfoComponent } from './employee-contact-info/employee-contact-info/employee-contact-info.component';
import { EmployeeContactInfoFormComponent } from './employee-contact-info/employee-contact-info-form/employee-contact-info-form.component';
import { DsCoreResourcesModule } from '@ds/core/resources/resources.module';
import { ElectronicConsentsModule } from '@ds/core/employees/electronic-consents/electronic-consents.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { JobProfileModalComponent } from './job-profile/job-profile-modal/job-profile-modal.component';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
import { DsCoreFormsModule } from '@ds/core/ui/forms';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};
@NgModule({
  imports: [
      MaterialModule,
      FormsModule,
      HttpClientModule,
      ReactiveFormsModule,
      CommonModule,
      DsExpansionModule,
      DsCardModule,
      DsCoreResourcesModule,
      ElectronicConsentsModule,
      LoadingMessageModule,
      DsCoreFormsModule,
      NgxMaskModule.forRoot(options)
  ],
    declarations: [EmployeeBioComponent, EmployeeBioFormComponent, EmployeeDependentsComponent, EmployeeDependentsFormComponent, EmergencyContactListComponent, EmergencyContactFormComponent, EmployeeContactInfoComponent, EmployeeContactInfoFormComponent, JobProfileModalComponent],
    entryComponents: [EmployeeBioFormComponent, EmployeeDependentsFormComponent, EmergencyContactFormComponent, EmployeeContactInfoFormComponent, JobProfileModalComponent],
    exports: [EmployeeBioComponent, EmployeeBioFormComponent, EmployeeDependentsComponent, EmployeeDependentsFormComponent, EmergencyContactListComponent, EmployeeContactInfoComponent]
})
export class DsEmployeeProfileModule { }
