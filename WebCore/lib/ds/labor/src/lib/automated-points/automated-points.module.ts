import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecalcPointsDialogComponent } from './recalc-points-dialog/recalc-points-dialog.component';
import { RecalcPointsTriggerComponent } from './recalc-points-trigger/recalc-points-trigger.component';
import { FormsModule } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { UpdateBalanceTriggerComponent } from './update-balance-trigger/update-balance-trigger.component';
import { UpdateBalanceDialogComponent } from './update-balance-dialog/update-balance-dialog.component';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { MaterialModule } from '@ds/core/ui/material';

@NgModule({
  declarations: [RecalcPointsDialogComponent, RecalcPointsTriggerComponent, UpdateBalanceTriggerComponent, UpdateBalanceDialogComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatIconModule,
    DsDialogModule,
    MaterialModule,
  ],
  exports: [RecalcPointsDialogComponent, RecalcPointsTriggerComponent]
})
export class AutomatedPointsModule { }
