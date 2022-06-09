import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeDirectDepositsComponent } from './employee-direct-deposits.component';

describe('EmployeeDirectDepositsComponent', () => {
  let component: EmployeeDirectDepositsComponent;
  let fixture: ComponentFixture<EmployeeDirectDepositsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeDirectDepositsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeDirectDepositsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
