import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalProcessSummaryDialogComponent } from './approval-process-summary-dialog.component';

describe('ApprovalProcessSummaryDialogComponent', () => {
  let component: ApprovalProcessSummaryDialogComponent;
  let fixture: ComponentFixture<ApprovalProcessSummaryDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovalProcessSummaryDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalProcessSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
