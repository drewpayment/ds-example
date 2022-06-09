import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { CompetencySetupComponent } from '@ds/performance/competencies/competency-setup/competency-setup.component';
import { CompetencyEditDialogComponent } from '@ds/performance/competencies/competency-edit-dialog/competency-edit-dialog.component';
import { DefaultCompetenciesDialogComponent } from '@ds/performance/competencies/default-competencies-dialog/default-competencies-dialog.component';
import { CompetencyDeleteConfirmDialogComponent } from '@ds/performance/competencies/competency-delete-confirm-dialog/competency-delete-confirm-dialog.component';
import { CompetencyModelComponent } from '@ds/performance/competencies/competency-model/competency-model.component';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { CompetencyModelEditDialogComponent } from '@ds/performance/competencies/competency-model-edit-dialog/competency-model-edit-dialog.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { CompetencyModelAssignEmployeeComponent } from '@ds/performance/competencies/competency-model-assign-employee/competency-model-assign-employee.component';
import { CompetencyComponent } from '@ds/performance/competencies/competency/competency.component';
import { CompetencyListComponent } from '@ds/performance/competencies/competency-list/competency-list.component';
import { CompetencyModelAssignmentComponent } from '@ds/performance/competencies/competency-model-assignment/competency-model-assignment.component';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsCardModule,
  ],
  declarations: [
    CompetencySetupComponent,
    CompetencyEditDialogComponent,
    DefaultCompetenciesDialogComponent,
    CompetencyDeleteConfirmDialogComponent,
    CompetencyModelComponent,
    CompetencyModelEditDialogComponent,
    CompetencyModelAssignEmployeeComponent,
    CompetencyComponent,
    CompetencyListComponent,
    CompetencyModelAssignmentComponent,
  ],
  entryComponents: [CompetencyModelEditDialogComponent],
  exports: [
    CompetencySetupComponent,
    CompetencyEditDialogComponent,
    DefaultCompetenciesDialogComponent,
    CompetencyDeleteConfirmDialogComponent,
    CompetencyModelComponent,
    CompetencyModelEditDialogComponent,
    CompetencyComponent,
    CompetencyListComponent,
    CompetencyModelAssignmentComponent,
  ],
})
export class CompetenciesModule {}
