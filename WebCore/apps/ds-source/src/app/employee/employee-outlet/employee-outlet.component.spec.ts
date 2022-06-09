import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeOutletComponent } from './employee-outlet.component';

describe('EmployeeOutletComponent', () => {
  let component: EmployeeOutletComponent;
  let fixture: ComponentFixture<EmployeeOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
