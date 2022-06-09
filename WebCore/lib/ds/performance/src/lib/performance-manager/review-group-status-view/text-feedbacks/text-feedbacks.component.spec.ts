import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TextFeedbacksComponent } from './text-feedbacks.component';

describe('TextFeedbacksComponent', () => {
  let component: TextFeedbacksComponent;
  let fixture: ComponentFixture<TextFeedbacksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TextFeedbacksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TextFeedbacksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});