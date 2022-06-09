import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsFirstYearAccrualSchedulesCardComponent } from './client-accruals-first-year-accrual-schedules-card.component';

describe('ClientAccrualsFirstYearAccrualSchedulesCardComponent', () => {
  let component: ClientAccrualsFirstYearAccrualSchedulesCardComponent;
  let fixture: ComponentFixture<ClientAccrualsFirstYearAccrualSchedulesCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsFirstYearAccrualSchedulesCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsFirstYearAccrualSchedulesCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
