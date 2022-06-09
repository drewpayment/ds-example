import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDependentsFormComponent } from './employee-dependents-form.component';

describe('EmployeeDependentsFormComponent', () => {
  let component: EmployeeDependentsFormComponent;
  let fixture: ComponentFixture<EmployeeDependentsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDependentsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDependentsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
