import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDirectDepositsFormComponent } from './employee-direct-deposits-form.component';

describe('EmployeeDirectDepositsFormComponent', () => {
  let component: EmployeeDirectDepositsFormComponent;
  let fixture: ComponentFixture<EmployeeDirectDepositsFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDirectDepositsFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDirectDepositsFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
