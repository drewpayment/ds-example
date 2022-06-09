import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaycheckOutletComponent } from './paycheck-outlet.component';

describe('PaycheckListComponent', () => {
  let component: PaycheckOutletComponent;
  let fixture: ComponentFixture<PaycheckOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaycheckOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaycheckOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
