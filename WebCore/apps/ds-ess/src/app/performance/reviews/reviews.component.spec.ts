import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeReviewsComponent } from './reviews.component';

describe('ReviewsComponent', () => {
  let component: EmployeeReviewsComponent;
  let fixture: ComponentFixture<EmployeeReviewsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeReviewsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeReviewsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
