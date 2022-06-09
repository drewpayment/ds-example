import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalWeightingDialogComponent } from './goal-weighting-dialog.component';

describe('GoalWeightingDialogComponent', () => {
  let component: GoalWeightingDialogComponent;
  let fixture: ComponentFixture<GoalWeightingDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoalWeightingDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoalWeightingDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
