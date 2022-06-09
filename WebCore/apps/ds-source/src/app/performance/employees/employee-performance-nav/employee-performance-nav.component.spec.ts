import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePerformanceNavComponent } from './employee-performance-nav.component';

describe('EmployeePerformanceNavComponent', () => {
  let component: EmployeePerformanceNavComponent;
  let fixture: ComponentFixture<EmployeePerformanceNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePerformanceNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePerformanceNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
