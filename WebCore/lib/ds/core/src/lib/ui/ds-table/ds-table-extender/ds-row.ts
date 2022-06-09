/**
 * @license
 * Copyright Google LLC All Rights Reserved.
 *
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file at https://angular.io/license
 */

 import {
  CDK_ROW_TEMPLATE,
  CdkFooterRow,
  CdkFooterRowDef,
  CdkHeaderRow,
  CdkHeaderRowDef,
  CdkRow,
  CdkRowDef,
  CdkNoDataRow
} from '@angular/cdk/table';
import {ChangeDetectionStrategy, Component, Directive, HostBinding, ViewEncapsulation} from '@angular/core';

/**
 * Header row definition for the mat-table.
 * Captures the header row's template and other header properties such as the columns to display.
 */
@Directive({
  selector: '[dsHeaderRowDef]',
  providers: [{provide: CdkHeaderRowDef, useExisting: DsHeaderRowDef}],
  inputs: ['columns: dsHeaderRowDef', 'sticky: dsHeaderRowDefSticky'],
})
export class DsHeaderRowDef extends CdkHeaderRowDef {}

/**
 * Footer row definition for the mat-table.
 * Captures the footer row's template and other footer properties such as the columns to display.
 */
@Directive({
  selector: '[dsFooterRowDef]',
  providers: [{provide: CdkFooterRowDef, useExisting: DsFooterRowDef}],
  inputs: ['columns: dsFooterRowDef', 'sticky: dsFooterRowDefSticky'],
})
export class DsFooterRowDef extends CdkFooterRowDef {}

/**
 * Data row definition for the mat-table.
 * Captures the data row's template and other properties such as the columns to display and
 * a when predicate that describes when this row should be used.
 */
@Directive({
  selector: '[dsRowDef]',
  providers: [{provide: CdkRowDef, useExisting: DsRowDef}],
  inputs: ['columns: dsRowDefColumns', 'when: dsRowDefWhen'],
})
export class DsRowDef<T> extends CdkRowDef<T> {
}

/** Header template container that contains the cell outlet. Adds the right class and role. */
@Component({
  selector: 'ds-header-row, tr[ds-header-row]',
  template: CDK_ROW_TEMPLATE,
  host: {
    'class': 'ds-header-row',
    'role': 'row',
  },
  // See note on CdkTable for explanation on why this uses the default change detection strategy.
  // tslint:disable-next-line:validate-decorators
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
  exportAs: 'dsHeaderRow',
  providers: [{provide: CdkHeaderRow, useExisting: DsHeaderRow}],
})
export class DsHeaderRow extends CdkHeaderRow {
}

/** Footer template container that contains the cell outlet. Adds the right class and role. */
@Component({
  selector: 'ds-footer-row, tr[ds-footer-row]',
  template: CDK_ROW_TEMPLATE,
  host: {
    'class': 'ds-footer-row',
    'role': 'row',
  },
  // See note on CdkTable for explanation on why this uses the default change detection strategy.
  // tslint:disable-next-line:validate-decorators
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
  exportAs: 'dsFooterRow',
  providers: [{provide: CdkFooterRow, useExisting: DsFooterRow}],
})
export class DsFooterRow extends CdkFooterRow {
}

/** Data row template container that contains the cell outlet. Adds the right class and role. */
@Component({
  selector: 'ds-row, tr[ds-row]',
  template: CDK_ROW_TEMPLATE,
  host: {
    'class': 'ds-row',
    'role': 'row',
  },
  // See note on CdkTable for explanation on why this uses the default change detection strategy.
  // tslint:disable-next-line:validate-decorators
  changeDetection: ChangeDetectionStrategy.Default,
  encapsulation: ViewEncapsulation.None,
  exportAs: 'dsRow',
  providers: [{provide: CdkRow, useExisting: DsRow}],
})
export class DsRow extends CdkRow {
}

/** Row that can be used to display a message when no data is shown in the table. */
@Directive({
  selector: '[dsNoDataRow]',
  providers: [{provide: CdkNoDataRow, useExisting: DsNoDataRow}],
  host:      {'class': 'empty'}
})
export class DsNoDataRow extends CdkNoDataRow {
  
}