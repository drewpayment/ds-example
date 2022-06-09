import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PayrollRequestsComponent } from './payroll-requests.component';

describe('PayrollRequestsComponent', () => {
  let component: PayrollRequestsComponent;
  let fixture: ComponentFixture<PayrollRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PayrollRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PayrollRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
