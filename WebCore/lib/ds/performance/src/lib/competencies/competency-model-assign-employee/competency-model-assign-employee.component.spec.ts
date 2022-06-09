import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyModelAssignEmployeeComponent } from './competency-model-assign-employee.component';

describe('CompetencyModelAssignEmployeeComponent', () => {
  let component: CompetencyModelAssignEmployeeComponent;
  let fixture: ComponentFixture<CompetencyModelAssignEmployeeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyModelAssignEmployeeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyModelAssignEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
