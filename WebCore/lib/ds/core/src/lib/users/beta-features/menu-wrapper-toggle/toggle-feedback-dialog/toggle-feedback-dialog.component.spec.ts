import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ToggleFeedbackDialogComponent } from './toggle-feedback-dialog.component';

describe('ToggleFeedbackDialogComponent', () => {
  let component: ToggleFeedbackDialogComponent;
  let fixture: ComponentFixture<ToggleFeedbackDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ToggleFeedbackDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToggleFeedbackDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
