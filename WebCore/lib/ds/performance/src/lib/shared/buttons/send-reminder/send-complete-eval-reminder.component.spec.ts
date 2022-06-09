import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SendCompleteEvalReminderComponent } from './send-complete-eval-reminder.component';

describe('SendReminderComponent', () => {
  let component: SendCompleteEvalReminderComponent;
  let fixture: ComponentFixture<SendCompleteEvalReminderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SendCompleteEvalReminderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SendCompleteEvalReminderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
