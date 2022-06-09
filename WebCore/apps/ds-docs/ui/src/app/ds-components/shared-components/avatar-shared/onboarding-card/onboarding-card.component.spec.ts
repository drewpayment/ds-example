import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnboardingCardComponent } from './onboarding-card.component';

describe('OnboardingCardComponent', () => {
  let component: OnboardingCardComponent;
  let fixture: ComponentFixture<OnboardingCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnboardingCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnboardingCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
