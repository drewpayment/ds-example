import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeServicesComponent } from './employee-services.component';

describe('EmployeeServicesComponent', () => {
  let component: EmployeeServicesComponent;
  let fixture: ComponentFixture<EmployeeServicesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeServicesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeServicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
