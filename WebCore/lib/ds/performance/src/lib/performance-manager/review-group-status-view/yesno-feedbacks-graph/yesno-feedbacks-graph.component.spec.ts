import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { YesNoFeedbacksGraphComponent } from './yesno-feedbacks-graph.component';

describe('YesNoFeedbacksGraphComponent', () => {
  let component: YesNoFeedbacksGraphComponent;
  let fixture: ComponentFixture<YesNoFeedbacksGraphComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ YesNoFeedbacksGraphComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(YesNoFeedbacksGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});