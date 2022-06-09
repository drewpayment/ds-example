import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { PayrollRequestHeaderData } from '../../shared/report-display-data.model';

@Component({
  selector: 'ds-payroll-request-report-header',
  templateUrl: './payroll-request-report-header.component.html',
  styleUrls: ['./payroll-request-report-header.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PayrollRequestReportHeaderComponent {

@Input() headerData: PayrollRequestHeaderData

  constructor() { }

}
