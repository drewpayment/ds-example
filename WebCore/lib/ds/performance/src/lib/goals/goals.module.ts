import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '@ds/core/ui/material/material.module';
import { AddGoalDialogComponent } from '@ds/performance/goals/add-goal-dialog/add-goal-dialog.component';
import { GoalDetailComponent } from '@ds/performance/goals/goal-detail/goal-detail.component';
import { EmployeeGoalsListComponent } from '@ds/performance/goals/employee-goals-list/employee-goals-list.component';
import { AjsUpgradesModule } from '@ds/core/ajs-upgrades/ajs-upgrades.module';
import { CompanyGoalsListComponent } from '@ds/performance/goals/company-goals-list/company-goals-list.component';
import { GoalStatusCardComponent } from '@ds/performance/goals/goal-status-card/goal-status-card.component';
import { DsExpansionModule } from '@ds/core/ui/ds-expansion';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { DsProgressModule } from '@ds/core/ui/ds-progress';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { FilterGoalsPipe } from '@ds/performance/goals/shared/filter-goals.pipe';
import { DsAutocompleteModule } from '@ds/core/ui/ds-autocomplete';
import { DeleteGoalDialogComponent } from '@ds/performance/goals/delete-goal-dialog/delete-goal-dialog.component';
import { AvatarModule } from '@ds/core/ui/avatar/avatar.module';
import { FormatDescriptionPipe } from '@ds/performance/shared/format-description.pipe';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    AjsUpgradesModule,
    DsExpansionModule,
    DsCardModule,
    DsProgressModule,
    DsCoreFormsModule,
    DsAutocompleteModule,
    AvatarModule,
  ],
  declarations: [
    AddGoalDialogComponent,
    GoalDetailComponent,
    EmployeeGoalsListComponent,
    CompanyGoalsListComponent,
    GoalStatusCardComponent,
    FilterGoalsPipe,
    DeleteGoalDialogComponent,
    FormatDescriptionPipe,
  ],
  exports: [
    AddGoalDialogComponent,
    GoalDetailComponent,
    EmployeeGoalsListComponent,
    CompanyGoalsListComponent,
    GoalStatusCardComponent,
    FormatDescriptionPipe,
  ],
  providers: [FormatDescriptionPipe],
  entryComponents: [AddGoalDialogComponent, DeleteGoalDialogComponent],
})
export class GoalsModule {}
