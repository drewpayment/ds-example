import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeGoalsListComponent } from './employee-goals-list.component';

describe('EmployeeGoalsListComponent', () => {
  let component: EmployeeGoalsListComponent;
  let fixture: ComponentFixture<EmployeeGoalsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeGoalsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeGoalsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
