import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentCountWidgetComponent } from './payment-count-widget.component';

describe('PaymentCountWidgetComponent', () => {
  let component: PaymentCountWidgetComponent;
  let fixture: ComponentFixture<PaymentCountWidgetComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentCountWidgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentCountWidgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
