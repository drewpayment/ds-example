import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewProfilesOutletComponent } from './review-profiles-outlet.component';

describe('ReviewProfilesOutletComponent', () => {
  let component: ReviewProfilesOutletComponent;
  let fixture: ComponentFixture<ReviewProfilesOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewProfilesOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewProfilesOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
