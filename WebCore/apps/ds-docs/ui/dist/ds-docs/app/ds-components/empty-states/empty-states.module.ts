import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmptyStatesDocsComponent } from './empty-states-docs/empty-states-docs.component';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { MarkdownModule } from 'ngx-markdown';
import { EmptyListComponent } from './empty-list/empty-list.component';
import { EmptyListReocurringComponent } from './empty-list-reocurring/empty-list-reocurring.component';
import { KanbanComponent } from './kanban/kanban.component';
import { DsCardModule } from '@ds/core/ui/ds-card';
import { AddTileComponent } from './add-tile/add-tile.component';
import { PayrollClosedComponent } from './payroll-closed/payroll-closed.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { EmptyStatesVerticalComponent } from './empty-states-vertical/empty-states-vertical.component';

@NgModule({
  declarations: [EmptyStatesDocsComponent, EmptyListComponent, EmptyListReocurringComponent, KanbanComponent, AddTileComponent, PayrollClosedComponent, PermissionsComponent, EmptyStatesVerticalComponent],
  imports: [
    CommonModule,
    DocsMaterialModule,
    MarkdownModule.forChild(),
    DsCardModule
  ]
})
export class EmptyStatesModule { }
