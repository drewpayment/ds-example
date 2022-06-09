import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeesRatingListDialogComponent } from './employees-rating-list-dialog.component';

describe('EmployeesRatingListComponent', () => {
  let component: EmployeesRatingListDialogComponent;
  let fixture: ComponentFixture<EmployeesRatingListDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeesRatingListDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeesRatingListDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
