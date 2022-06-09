import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsAccrualSchedulesCardComponent } from './client-accruals-accrual-schedules-card.component';

describe('ClientAccrualsAccrualSchedulesCardComponent', () => {
  let component: ClientAccrualsAccrualSchedulesCardComponent;
  let fixture: ComponentFixture<ClientAccrualsAccrualSchedulesCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsAccrualSchedulesCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsAccrualSchedulesCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
