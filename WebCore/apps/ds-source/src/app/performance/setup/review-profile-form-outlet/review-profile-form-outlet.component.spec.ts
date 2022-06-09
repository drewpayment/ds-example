import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewProfileFormOutletComponent } from './review-profile-form-outlet.component';

describe('ReviewProfileFormOutletComponent', () => {
  let component: ReviewProfileFormOutletComponent;
  let fixture: ComponentFixture<ReviewProfileFormOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewProfileFormOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewProfileFormOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
