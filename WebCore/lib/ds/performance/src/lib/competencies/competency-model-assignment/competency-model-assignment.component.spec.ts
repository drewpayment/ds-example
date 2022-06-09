import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyModelAssignmentComponent } from './competency-model-assignment.component';

describe('CompetencyModelAssignmentComponent', () => {
  let component: CompetencyModelAssignmentComponent;
  let fixture: ComponentFixture<CompetencyModelAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyModelAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyModelAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
