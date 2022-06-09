import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteGoalDialogComponent } from './delete-goal-dialog.component';

describe('DeleteGoalDialogComponent', () => {
  let component: DeleteGoalDialogComponent;
  let fixture: ComponentFixture<DeleteGoalDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteGoalDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteGoalDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
