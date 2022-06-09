import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReviewCompetencyGraphComponent } from './review-competency-graph.component';

describe('ReviewCompetencyGraphComponent', () => {
  let component: ReviewCompetencyGraphComponent;
  let fixture: ComponentFixture<ReviewCompetencyGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReviewCompetencyGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReviewCompetencyGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
