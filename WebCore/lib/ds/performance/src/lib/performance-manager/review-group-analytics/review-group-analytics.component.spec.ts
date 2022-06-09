import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewGroupAnalyticsComponent } from './review-group-analytics.component';

describe('ReviewGroupAnalyticsComponent', () => {
  let component: ReviewGroupAnalyticsComponent;
  let fixture: ComponentFixture<ReviewGroupAnalyticsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewGroupAnalyticsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewGroupAnalyticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
