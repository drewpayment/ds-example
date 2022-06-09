import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollRequestEmpSectionComponent } from './payroll-request-emp-section.component';

describe('PayrollRequestEmpSectionComponent', () => {
  let component: PayrollRequestEmpSectionComponent;
  let fixture: ComponentFixture<PayrollRequestEmpSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollRequestEmpSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollRequestEmpSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
