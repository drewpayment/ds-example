import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArDominionCheckPaymentComponent } from './ar-dominion-check-payment.component';

describe('ArDominionCheckPaymentComponent', () => {
  let component: ArDominionCheckPaymentComponent;
  let fixture: ComponentFixture<ArDominionCheckPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArDominionCheckPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArDominionCheckPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
