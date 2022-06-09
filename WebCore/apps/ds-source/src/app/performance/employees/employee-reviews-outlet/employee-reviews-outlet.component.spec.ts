import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeReviewsOutletComponent } from './employee-reviews-outlet.component';

describe('EmployeeReviewsOutletComponent', () => {
  let component: EmployeeReviewsOutletComponent;
  let fixture: ComponentFixture<EmployeeReviewsOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeReviewsOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeReviewsOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
