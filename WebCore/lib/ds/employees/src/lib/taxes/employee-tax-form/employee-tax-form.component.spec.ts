import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTaxFormComponent } from './employee-tax-form.component';

describe('EmployeeTaxFormComponent', () => {
  let component: EmployeeTaxFormComponent;
  let fixture: ComponentFixture<EmployeeTaxFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTaxFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTaxFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
