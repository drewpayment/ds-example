import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeSchedulerOutletComponent } from './employee-scheduler-outlet.component';

describe('EmployeeSchedulerOutletComponent', () => {
  let component: EmployeeSchedulerOutletComponent;
  let fixture: ComponentFixture<EmployeeSchedulerOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeSchedulerOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeSchedulerOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
