import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollRequestReportComponent } from './payroll-request-report.component';

describe('PayrollRequestReportComponent', () => {
  let component: PayrollRequestReportComponent;
  let fixture: ComponentFixture<PayrollRequestReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollRequestReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollRequestReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
