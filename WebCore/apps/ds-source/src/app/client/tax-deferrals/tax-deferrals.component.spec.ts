import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaxDeferralsComponent } from './tax-deferrals.component';

describe('TaxDeferralComponent', () => {
  let component: TaxDeferralsComponent;
  let fixture: ComponentFixture<TaxDeferralsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaxDeferralsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaxDeferralsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
