import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvanceEnrollmentReportDialogComponent } from './advance-enrollment-report-dialog.component';

describe('AdvanceEnrollmentReportDialogComponent', () => {
  let component: AdvanceEnrollmentReportDialogComponent;
  let fixture: ComponentFixture<AdvanceEnrollmentReportDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvanceEnrollmentReportDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvanceEnrollmentReportDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
