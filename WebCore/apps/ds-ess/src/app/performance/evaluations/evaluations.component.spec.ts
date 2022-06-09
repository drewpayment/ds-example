import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeEvaluationViewComponent } from './evaluations.component';

describe('EvaluationsComponent', () => {
  let component: EmployeeEvaluationViewComponent;
  let fixture: ComponentFixture<EmployeeEvaluationViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeEvaluationViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeEvaluationViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
