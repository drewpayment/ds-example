import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTaxListComponent } from './employee-tax-list.component';

describe('EmployeeTaxListComponent', () => {
  let component: EmployeeTaxListComponent;
  let fixture: ComponentFixture<EmployeeTaxListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTaxListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTaxListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
