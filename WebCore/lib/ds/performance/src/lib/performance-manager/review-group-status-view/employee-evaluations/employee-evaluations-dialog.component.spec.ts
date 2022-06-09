import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeEvaluationsDialogComponent } from './employee-evaluations-dialog.component';

describe('EmployeeEvaluationsDialogComponent', () => {
  let component: EmployeeEvaluationsDialogComponent;
  let fixture: ComponentFixture<EmployeeEvaluationsDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeEvaluationsDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeEvaluationsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});