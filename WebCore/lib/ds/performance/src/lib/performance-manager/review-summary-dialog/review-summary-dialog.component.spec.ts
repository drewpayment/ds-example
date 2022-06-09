import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewSummaryDialogComponent } from './review-summary-dialog.component';

describe('ReviewSummaryDialogComponent', () => {
  let component: ReviewSummaryDialogComponent;
  let fixture: ComponentFixture<ReviewSummaryDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewSummaryDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
