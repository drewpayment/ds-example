import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutomaticBillingComponent } from './automatic-billing.component';

describe('AutomaticBillingComponent', () => {
  let component: AutomaticBillingComponent;
  let fixture: ComponentFixture<AutomaticBillingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutomaticBillingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutomaticBillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
