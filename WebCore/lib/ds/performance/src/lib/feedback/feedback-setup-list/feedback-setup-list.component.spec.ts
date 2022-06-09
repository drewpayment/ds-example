import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedbackSetupListComponent } from './feedback-setup-list.component';

describe('FeedbackSetupListComponent', () => {
  let component: FeedbackSetupListComponent;
  let fixture: ComponentFixture<FeedbackSetupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeedbackSetupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedbackSetupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
