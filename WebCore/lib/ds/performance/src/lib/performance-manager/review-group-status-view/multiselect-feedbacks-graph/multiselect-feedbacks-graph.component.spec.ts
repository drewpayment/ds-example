import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiSelectFeedbacksGraphComponent } from './multiselect-feedbacks-graph.component';

describe('MultiSelectFeedbacksGraphComponent', () => {
  let component: MultiSelectFeedbacksGraphComponent;
  let fixture: ComponentFixture<MultiSelectFeedbacksGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiSelectFeedbacksGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiSelectFeedbacksGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});