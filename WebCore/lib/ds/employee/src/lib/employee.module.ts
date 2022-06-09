import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { EmployeeDeductionsComponent } from './employee-deductions/employee-deductions.component';
import { DeductionsComponent } from './employee-deductions/deductions/deductions.component';
import { DirectDepositComponent } from './employee-deductions/direct-deposit/direct-deposit.component';
import { EarningsComponent } from './employee-deductions/earnings/earnings.component';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '@ds/core/ui/material';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DeductionsAddEditModalComponent } from './employee-deductions/deductions/deductions-add-edit-modal/deductions-add-edit-modal.component';
import { DirectDepositAddEditModalComponent } from './employee-deductions/direct-deposit/direct-deposit-add-edit-modal/direct-deposit-add-edit-modal.component';
import { EarningsAddEditModalComponent } from './employee-deductions/earnings/earnings-add-edit-modal/earnings-add-edit-modal.component';

@NgModule({
  declarations: [
    EmployeeDeductionsComponent,
    DeductionsComponent,
    DirectDepositComponent,
    EarningsComponent,
    DeductionsAddEditModalComponent,
    DirectDepositAddEditModalComponent,
    EarningsAddEditModalComponent,
  ],
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsCardModule,
  ],
  entryComponents: [
    DeductionsAddEditModalComponent,
    DirectDepositAddEditModalComponent,
    EarningsAddEditModalComponent,
  ],
})
export class EmployeeModule {}
