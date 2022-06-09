import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BankDepositWidgetComponent } from './bank-deposit-widget.component';

describe('BankDepositWidgetComponent', () => {
  let component: BankDepositWidgetComponent;
  let fixture: ComponentFixture<BankDepositWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BankDepositWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BankDepositWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
