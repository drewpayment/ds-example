import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewMeetingDialogComponent } from './review-meeting-dialog.component';

describe('ReviewMeetingDialogComponent', () => {
  let component: ReviewMeetingDialogComponent;
  let fixture: ComponentFixture<ReviewMeetingDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewMeetingDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewMeetingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
