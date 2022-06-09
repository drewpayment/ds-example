import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewProfileSetupFormComponent } from './review-profile-setup-form.component';

describe('ReviewProfileSetupFormComponent', () => {
  let component: ReviewProfileSetupFormComponent;
  let fixture: ComponentFixture<ReviewProfileSetupFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewProfileSetupFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewProfileSetupFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
