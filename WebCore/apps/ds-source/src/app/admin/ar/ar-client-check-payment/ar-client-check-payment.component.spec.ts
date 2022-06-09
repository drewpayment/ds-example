import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArClientCheckPaymentComponent } from './ar-client-check-payment.component';

describe('ArClientCheckPaymentComponent', () => {
  let component: ArClientCheckPaymentComponent;
  let fixture: ComponentFixture<ArClientCheckPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArClientCheckPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArClientCheckPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
