import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsEarningsCardComponent } from './client-accruals-earnings-card.component';

describe('ClientAccrualsEarningsCardComponent', () => {
  let component: ClientAccrualsEarningsCardComponent;
  let fixture: ComponentFixture<ClientAccrualsEarningsCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsEarningsCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsEarningsCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
