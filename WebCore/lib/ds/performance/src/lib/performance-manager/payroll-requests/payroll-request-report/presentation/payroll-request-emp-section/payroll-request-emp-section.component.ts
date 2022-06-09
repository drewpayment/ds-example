import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { EmpRequestSection } from '../../shared/report-display-data.model';
import { IncreaseType } from '@ds/performance/competencies/shared/increase-type';
import { PayTypeEnum } from '@ajs/employee/hiring/shared/models/employee-hire-data.interface';
import { ApprovalStatus } from '@ds/performance/evaluations/shared/approval-status.enum';

@Component({
  selector: 'ds-payroll-request-emp-section',
  templateUrl: './payroll-request-emp-section.component.html',
  styleUrls: ['./payroll-request-emp-section.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PayrollRequestEmpSectionComponent {

@Input() section: EmpRequestSection;
@Input() isScoringEnabled: boolean;

get ApprovalStatus(){
  return ApprovalStatus;
}

get IncreaseType() {
  return IncreaseType;
}

get PayType() {
  return PayTypeEnum;
}

  constructor() { }

}
