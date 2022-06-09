import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsPaidLeaveActCardComponent } from './client-accruals-paid-leave-act-card.component';

describe('ClientAccrualsPaidLeaveActCardComponent', () => {
  let component: ClientAccrualsPaidLeaveActCardComponent;
  let fixture: ComponentFixture<ClientAccrualsPaidLeaveActCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsPaidLeaveActCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsPaidLeaveActCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
