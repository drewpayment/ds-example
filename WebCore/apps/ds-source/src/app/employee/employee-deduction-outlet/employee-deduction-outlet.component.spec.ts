import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDeductionOutletComponent } from './employee-deduction-outlet.component';

describe('EmployeeDeductionOutletComponent', () => {
  let component: EmployeeDeductionOutletComponent;
  let fixture: ComponentFixture<EmployeeDeductionOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDeductionOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDeductionOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
