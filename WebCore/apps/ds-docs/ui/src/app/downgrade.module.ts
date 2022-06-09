import { DocsMaterialModule } from './material.module';
import { downgradeComponent } from '@angular/upgrade/static';
import * as angular from 'angular';
import { NgModule } from '@angular/core';

import { ComponentsDocsModule } from './components/components.module';
import { DatepickerComponent } from './components/datepicker/datepicker.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatTableDemoComponent,
  MatTableSortDemoComponent,
} from './components/tables/mat-components/mat-table-demo.component';
import { MatTablePaginationDemoComponent } from './components/tables/mat-components/mat-table-pagination-demo.component';
import { MatProgressBarDemoComponent } from './components/progress/mat-components/mat-progress-bar-demo.component';
// import { DsDialogExampleComponent, MatDialogTriggerDemoComponent } from './components/dialog/mat-components/mat-dialog-demo.component';
// import { DsDialogTabExampleComponent, MatDialogTriggerTabDemoComponent } from './components/dialog/mat-components/mat-dialog-demo.component';
import { DsProgressDowngradeModule } from '@ds/core/ui/ds-progress/ajs';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { ContactDemoModule } from './components/dsautocomplete/contact-demo/contact-demo.module';
import { RouterModule } from '@angular/router';
import { TerminateEmployeeModalTriggerComponent } from '@ajs/employee/terminate-employee/terminate-employee-modal/terminate-employee-modal-trigger.component';
import { DsHomeModule } from './ds-home/ds-home.module';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    CommonModule,
    DocsMaterialModule, // Must be imported AFTER BrowserModule
    FormsModule,
    ReactiveFormsModule,
    DsProgressDowngradeModule,
    DsDialogModule,
    ContactDemoModule,
    RouterModule,
    DsHomeModule,
  ],
  declarations: [
    DatepickerComponent,
    MatTableDemoComponent,
    MatTablePaginationDemoComponent,
    MatTableSortDemoComponent,
    MatProgressBarDemoComponent,
    // DsDialogExampleComponent,
    // MatDialogTriggerDemoComponent,
    // DsDialogTabExampleComponent,
    // MatDialogTriggerTabDemoComponent,
    TerminateEmployeeModalTriggerComponent,
  ],
  entryComponents: [
    DatepickerComponent,
    MatTableDemoComponent,
    MatTablePaginationDemoComponent,
    MatTableSortDemoComponent,
    MatProgressBarDemoComponent,
    // DsDialogExampleComponent,
    // MatDialogTriggerDemoComponent,
    // DsDialogTabExampleComponent,
    // MatDialogTriggerTabDemoComponent
  ],
  exports: [
    DatepickerComponent,
    MatTableDemoComponent,
    MatTablePaginationDemoComponent,
    MatTableSortDemoComponent,
    MatProgressBarDemoComponent,
    // DsDialogExampleComponent,
    // MatDialogTriggerDemoComponent,
    // DsDialogTabExampleComponent,
    // MatDialogTriggerTabDemoComponent
  ],
})
export class DowngradeModule {
  constructor() {}
}

angular
  .module(ComponentsDocsModule.AjsModule.name)
  .directive(
    'designDatepicker',
    downgradeComponent({
      component: DatepickerComponent,
    }) as angular.IDirectiveFactory
  )
  .directive(
    'matTableDemo',
    downgradeComponent({
      component: MatTableDemoComponent,
    }) as angular.IDirectiveFactory
  )
  .directive(
    'matTablePaginationDemo',
    downgradeComponent({
      component: MatTablePaginationDemoComponent,
    }) as angular.IDirectiveFactory
  )
  .directive(
    'matTableSortDemo',
    downgradeComponent({
      component: MatTableSortDemoComponent,
    }) as angular.IDirectiveFactory
  )
  .directive(
    'matProgressBarDemo',
    downgradeComponent({
      component: MatProgressBarDemoComponent,
    }) as angular.IDirectiveFactory
  );
