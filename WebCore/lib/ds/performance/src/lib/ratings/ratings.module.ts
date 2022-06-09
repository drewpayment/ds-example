import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { RatingsEditDialogComponent } from '@ds/performance/ratings/ratings-edit-dialog/ratings-edit-dialog.component';
import { RatingsEditComponent } from '@ds/performance/ratings/ratings-edit/ratings-edit.component';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { DsCardModule } from '@ds/core/ui/ds-card';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCardModule,
  ],
  declarations: [RatingsEditComponent, RatingsEditDialogComponent],
  exports: [RatingsEditComponent, RatingsEditDialogComponent],
})
export class RatingsModule {}
