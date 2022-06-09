import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { EmployeeTaxListComponent } from './employee-tax-list/employee-tax-list.component';
import { EmployeeTaxFormComponent } from './employee-tax-form/employee-tax-form.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { LoadingMessageModule } from '@ds/core/ui/loading-message/loading-message.module';
export const options: Partial<IConfig> | (() => Partial<IConfig>) = {};
@NgModule({
  imports: [
      MaterialModule,
      FormsModule,
      ReactiveFormsModule,
      CommonModule,
      DsExpansionModule,
      DsCardModule,
      LoadingMessageModule,

      NgxMaskModule.forRoot(options)
  ],
    declarations: [EmployeeTaxListComponent, EmployeeTaxFormComponent],
    entryComponents: [EmployeeTaxFormComponent],
    exports: [EmployeeTaxListComponent, EmployeeTaxFormComponent]
})
export class DsEmployeeTaxesModule { }
