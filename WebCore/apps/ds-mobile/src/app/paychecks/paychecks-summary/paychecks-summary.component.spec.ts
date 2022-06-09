import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaychecksSummaryComponent } from './paychecks-summary.component';

describe('PaychecksSummaryComponent', () => {
  let component: PaychecksSummaryComponent;
  let fixture: ComponentFixture<PaychecksSummaryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaychecksSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaychecksSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
