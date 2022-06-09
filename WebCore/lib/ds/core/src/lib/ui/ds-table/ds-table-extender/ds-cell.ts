/**
 * @license
 * Copyright Google LLC All Rights Reserved.
 *
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://angular.io/license
 */

 import {Directive, ElementRef, Input, Renderer2} from '@angular/core';
 import {
   CdkCell,
   CdkCellDef,
   CdkColumnDef, CdkFooterCell, CdkFooterCellDef,
   CdkHeaderCell,
   CdkHeaderCellDef,
 } from '@angular/cdk/table';
 
 /**
  * Cell definition for the mat-table.
  * Captures the template of a column's data row cell as well as cell-specific properties.
  */
 @Directive({
   selector: '[dsCellDef]',
   providers: [{provide: CdkCellDef, useExisting: DsCellDef}]
 })
 export class DsCellDef extends CdkCellDef {}
 
 /**
  * Header cell definition for the mat-table.
  * Captures the template of a column's header cell and as well as cell-specific properties.
  */
 @Directive({
   selector: '[dsHeaderCellDef]',
   providers: [{provide: CdkHeaderCellDef, useExisting: DsHeaderCellDef}]
 })
 export class DsHeaderCellDef extends CdkHeaderCellDef {}
 
 /**
  * Footer cell definition for the mat-table.
  * Captures the template of a column's footer cell and as well as cell-specific properties.
  */
 @Directive({
   selector: '[dsFooterCellDef]',
   providers: [{provide: CdkFooterCellDef, useExisting: DsFooterCellDef}]
 })
 export class DsFooterCellDef extends CdkFooterCellDef {}
 
 /**
  * Column definition for the mat-table.
  * Defines a set of cells available for a table column.
  */
 @Directive({
   selector: '[dsColumnDef]',
   inputs: ['sticky'],
   providers: [
     {provide: CdkColumnDef, useExisting: DsColumnDef},
     {provide: 'DS_SORT_HEADER_COLUMN_DEF', useExisting: DsColumnDef}
   ],
   host: {'class': 'ds-column'}
 })
 export class DsColumnDef extends CdkColumnDef {
  _columnCssClassName: string[] = [];
   
  //  override get name(): string { return this._name; }
  //  override set name(name: string) { this._setNameInput(name); }
 
  constructor(){
    super();
  }
  
  protected _setNameInput(value: string) {
    // If the directive is set without a name (updated programmatically), then this setter will
    // trigger with an empty string and should not overwrite the programmatically set value.
    if (value) {
      this._name = value;
      this.cssClassFriendlyName = value.replace(/[^a-z0-9_-]/ig, '-');
      this._updateColumnCssClassName();
    }
  }

   /** Unique name for this column. */
   @Input('dsColumnDef')
   get name(): string { return this._name; }
   set name(name: string) { this._setNameInput(name); }

   protected _updateColumnCssClassName() {
     this._columnCssClassName = [`ds-column-${this.cssClassFriendlyName}`];
   }
 }
 
 /** Header cell template container that adds the right classes and role. */
 @Directive({
   selector: 'ds-header-cell, th[ds-header-cell]',
   host: {
     'class': 'ds-header-cell',
     'role': 'columnheader',
   },
 })
 export class DsHeaderCell extends CdkHeaderCell {}
 
 /** Footer cell template container that adds the right classes and role. */
 @Directive({
   selector: 'ds-footer-cell, td[ds-footer-cell]',
   host: {
     'class': 'ds-footer-cell',
     'role': 'gridcell',
   },
 })
 export class DsFooterCell extends CdkFooterCell {}
 
 /** Cell template container that adds the right classes and role. */
 @Directive({
   selector: 'ds-cell, td[ds-cell]',
   host: {
     'class': 'ds-cell',
     'role': 'gridcell',
   },
 })
 export class DsCell extends CdkCell {}

 // Class directives
 @Directive({
  selector: '[alignRight]',
  host: {
    'class': 'right',
  },
})
export class AlignRight {}

@Directive({
  selector: '[alignCenter]',
  host: {
    'class': 'center',
  },
})
export class AlignCenter {}

@Directive({
  selector: '[alignViewRight]',
  host: {
    'class': 'view-right',
  },
})
export class AlignViewRight {}

@Directive({
  selector: '[tableCheckbox]',
  host: {
    'class': 'ds-table-checkbox',
  },
})
export class TableCheckbox {}

@Directive({
  selector: '[tableAction]',
  host: {
    'class': 'action',
  },
})
export class TableAction {}