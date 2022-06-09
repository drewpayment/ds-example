import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewStatusCardComponent } from './review-status-card.component';

describe('ReviewStatusCardComponent', () => {
  let component: ReviewStatusCardComponent;
  let fixture: ComponentFixture<ReviewStatusCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewStatusCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewStatusCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
