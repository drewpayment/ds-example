import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardingSummaryDialogComponent } from './onboarding-summary-dialog.component';

describe('OnboardingSummaryDialogComponent', () => {
  let component: OnboardingSummaryDialogComponent;
  let fixture: ComponentFixture<OnboardingSummaryDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnboardingSummaryDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardingSummaryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
