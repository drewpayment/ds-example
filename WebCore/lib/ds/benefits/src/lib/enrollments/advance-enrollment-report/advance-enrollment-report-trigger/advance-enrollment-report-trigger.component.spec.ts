import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvanceEnrollmentReportTriggerComponent } from './advance-enrollment-report-trigger.component';

describe('AdvanceEnrollmentReportTriggerComponent', () => {
  let component: AdvanceEnrollmentReportTriggerComponent;
  let fixture: ComponentFixture<AdvanceEnrollmentReportTriggerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvanceEnrollmentReportTriggerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvanceEnrollmentReportTriggerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
