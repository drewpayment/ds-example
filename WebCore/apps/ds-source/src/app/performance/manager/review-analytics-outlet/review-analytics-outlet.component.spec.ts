import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewAnalyticsOutletComponent } from './review-analytics-outlet.component';

describe('ReviewAnalyticsOutletComponent', () => {
  let component: ReviewAnalyticsOutletComponent;
  let fixture: ComponentFixture<ReviewAnalyticsOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewAnalyticsOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewAnalyticsOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
