import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollClosedComponent } from './payroll-closed.component';

describe('PayrollClosedComponent', () => {
  let component: PayrollClosedComponent;
  let fixture: ComponentFixture<PayrollClosedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollClosedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollClosedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
