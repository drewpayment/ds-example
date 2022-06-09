import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyDeleteConfirmDialogComponent } from './competency-delete-confirm-dialog.component';

describe('CompetencyDeleteConfirmDialogComponent', () => {
  let component: CompetencyDeleteConfirmDialogComponent;
  let fixture: ComponentFixture<CompetencyDeleteConfirmDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyDeleteConfirmDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyDeleteConfirmDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
