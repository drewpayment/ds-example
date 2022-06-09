import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MarkdownModule } from 'ngx-markdown';
import { TableDocsComponent } from './table-docs/table-docs.component';
import { DsCardModule } from '@ds/core/ui/ds-card/ds-card.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DsDialogModule } from '@ds/core/ui/ds-dialog';
import { TableAddComponent } from '@ds/docs/ds-components/tables/old/table-add/table-add.component';
import { DsCoreFormsModule } from '@ds/core/ui/forms/forms.module';
import { DsTableModule } from '@ds/core/ui/ds-table/ds-table.module';
import { TableViewComponent } from './table-view/table-view.component';
import { TableValidationComponent } from './table-validation/table-validation.component';
import { TableAddPageComponent } from '@ds/docs/ds-components/tables/table-add-edit/table-add-page.component';
import { TableViewStickyComponent } from '@ds/docs/ds-components/tables/old/table-view-sticky/table-view-sticky.component';
import { DocsMaterialModule } from '@ds/docs/material.module';
import { TableEmptyComponent } from './table-empty/table-empty.component';
import { MaterialModule } from '@ds/core/ui/material';

@NgModule({
  declarations: [
    TableDocsComponent, 
    TableAddComponent,
    TableAddPageComponent,
    TableViewComponent,
    TableValidationComponent,
    TableViewStickyComponent,
    TableEmptyComponent
  ],
  imports: [
    CommonModule,
    MarkdownModule.forChild(),
    MaterialModule,
    DsCardModule,
    DocsMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    DsDialogModule,
    DsCoreFormsModule,
    DsTableModule
  ],
  entryComponents: [
  ]
})
export class TablesModule { }
