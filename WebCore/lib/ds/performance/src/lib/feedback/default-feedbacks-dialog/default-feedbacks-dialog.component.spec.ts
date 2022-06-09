import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultFeedbacksDialogComponent } from './default-feedbacks-dialog.component';

describe('DefaultFeedbacksDialogComponent', () => {
    let component: DefaultFeedbacksDialogComponent;
    let fixture: ComponentFixture<DefaultFeedbacksDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [ DefaultFeedbacksDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(DefaultFeedbacksDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
