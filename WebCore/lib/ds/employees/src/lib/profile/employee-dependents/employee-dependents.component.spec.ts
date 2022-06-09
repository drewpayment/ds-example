import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDependentsComponent } from './employee-dependents.component';

describe('EmployeeDependentsComponent', () => {
  let component: EmployeeDependentsComponent;
  let fixture: ComponentFixture<EmployeeDependentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDependentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDependentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
