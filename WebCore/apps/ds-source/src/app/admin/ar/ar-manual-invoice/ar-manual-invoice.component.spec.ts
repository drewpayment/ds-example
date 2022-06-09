import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArManualInvoiceComponent } from './ar-manual-invoice.component';

describe('ArManualInvoiceComponent', () => {
  let component: ArManualInvoiceComponent;
  let fixture: ComponentFixture<ArManualInvoiceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArManualInvoiceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArManualInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
