import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewScoringGraphComponent } from './review-scoring-graph.component';

describe('ReviewScoringGraphComponent', () => {
  let component: ReviewScoringGraphComponent;
  let fixture: ComponentFixture<ReviewScoringGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewScoringGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewScoringGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
