import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CdkTableModule } from '@angular/cdk/table';
import { AlignCenter, AlignRight, AlignViewRight, DsCell, DsCellDef, DsColumnDef, DsFooterCell, DsFooterCellDef, DsHeaderCell, DsHeaderCellDef, TableAction, TableCheckbox, } from './ds-table-extender/ds-cell';
import { DsTableExtender } from './ds-table-extender/ds-table-extender.component';
import { DsTextColumn } from './ds-table-extender/ds-column';
import { addButton, DsTableComponent } from './ds-table.component';
import { DsHeaderRow, DsHeaderRowDef, DsFooterRow, DsFooterRowDef, DsRowDef, DsRow, DsNoDataRow } from './ds-table-extender/ds-row';


const EXPORTED_DECLARATIONS = [
  DsTableExtender,
  DsTableComponent,

  DsHeaderRow,
  DsHeaderRowDef,

  DsHeaderCell,
  DsHeaderCellDef,

  DsFooterRow,
  DsFooterRowDef,
  DsNoDataRow,

  DsFooterCell,
  DsFooterCellDef,

  DsRowDef,
  DsRow,

  DsCellDef,
  DsCell,

  DsColumnDef,
  DsTextColumn,

  addButton,
  AlignRight,
  AlignCenter,
  AlignViewRight,
  TableCheckbox,
  TableAction
]

@NgModule({
  imports: [
    CommonModule,
    CdkTableModule
  ],
  declarations: EXPORTED_DECLARATIONS,
  providers: [
  ],
  exports: EXPORTED_DECLARATIONS,
})
export class DsTableModule { }
