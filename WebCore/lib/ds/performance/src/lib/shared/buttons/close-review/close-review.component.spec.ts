import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CloseReviewComponent } from './close-review.component';

describe('CloseReviewComponent', () => {
  let component: CloseReviewComponent;
  let fixture: ComponentFixture<CloseReviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CloseReviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CloseReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
