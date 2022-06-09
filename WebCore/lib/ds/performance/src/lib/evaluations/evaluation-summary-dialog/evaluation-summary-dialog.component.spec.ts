import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EvaluationSummaryDialogComponent } from './evaluation-summary-dialog.component';

describe('EvaluationSummaryDialogComponent', () => {
  let component: EvaluationSummaryDialogComponent;
  let fixture: ComponentFixture<EvaluationSummaryDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EvaluationSummaryDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EvaluationSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
