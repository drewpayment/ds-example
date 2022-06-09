import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollRequestReportHeaderComponent } from './payroll-request-report-header.component';

describe('PayrollRequestReportHeaderComponent', () => {
  let component: PayrollRequestReportHeaderComponent;
  let fixture: ComponentFixture<PayrollRequestReportHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollRequestReportHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollRequestReportHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
