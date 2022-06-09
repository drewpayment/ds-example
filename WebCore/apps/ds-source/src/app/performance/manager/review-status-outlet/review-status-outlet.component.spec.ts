import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewStatusOutletComponent } from './review-status-outlet.component';

describe('ReviewStatusOutletComponent', () => {
  let component: ReviewStatusOutletComponent;
  let fixture: ComponentFixture<ReviewStatusOutletComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewStatusOutletComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewStatusOutletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
