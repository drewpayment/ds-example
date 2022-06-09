import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GoalStatusCardComponent } from './goal-status-card.component';

describe('GoalStatusCardComponent', () => {
  let component: GoalStatusCardComponent;
  let fixture: ComponentFixture<GoalStatusCardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GoalStatusCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoalStatusCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
