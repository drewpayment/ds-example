import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientAccrualsEligibilityCardComponent } from './client-accruals-eligibility-card.component';

describe('ClientAccrualsEligibilityCardComponent', () => {
  let component: ClientAccrualsEligibilityCardComponent;
  let fixture: ComponentFixture<ClientAccrualsEligibilityCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientAccrualsEligibilityCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientAccrualsEligibilityCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
