import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvaluationPayrollRequestComponent } from './evaluation-payroll-request.component';

describe('EvaluationPayrollRequestComponent', () => {
  let component: EvaluationPayrollRequestComponent;
  let fixture: ComponentFixture<EvaluationPayrollRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvaluationPayrollRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvaluationPayrollRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
