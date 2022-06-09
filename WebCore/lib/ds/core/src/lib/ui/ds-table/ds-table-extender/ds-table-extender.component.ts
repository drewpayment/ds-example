import { 
  CdkTable, 
  CDK_TABLE_TEMPLATE 
} from '@angular/cdk/table';
import { ChangeDetectionStrategy, Component, Directive, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'ds-table, table[ds-table]',
  exportAs: 'dsTable',
  template: CDK_TABLE_TEMPLATE,
  styleUrls: ['./ds-table-extender.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [
    { provide: CdkTable, useExisting: DsTableExtender }
  ],
  host: {
    'class': 'ds-table', 
  }
})
export class DsTableExtender<T> extends CdkTable<T> {
}

@Directive({
  selector: '[stickyTable]',
  host: {
    'class': 'sticky'
  }
})
export class stickyTable {}
