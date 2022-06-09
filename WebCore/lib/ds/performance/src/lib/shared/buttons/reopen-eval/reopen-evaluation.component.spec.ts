import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReopenEvaluationComponent } from './reopen-evaluation.component';

describe('ReopenReviewComponent', () => {
  let component: ReopenEvaluationComponent;
  let fixture: ComponentFixture<ReopenEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReopenEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReopenEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
