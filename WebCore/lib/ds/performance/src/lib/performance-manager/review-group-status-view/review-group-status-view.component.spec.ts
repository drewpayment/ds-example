import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewGroupStatusViewComponent } from './review-group-status-view.component';

describe('ReviewGroupStatusViewComponent', () => {
  let component: ReviewGroupStatusViewComponent;
  let fixture: ComponentFixture<ReviewGroupStatusViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewGroupStatusViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewGroupStatusViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
