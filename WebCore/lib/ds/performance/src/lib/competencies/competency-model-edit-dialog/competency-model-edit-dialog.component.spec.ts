import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyModelEditDialogComponent } from './competency-model-edit-dialog.component';

describe('CompetencyModelEditDialogComponent', () => {
  let component: CompetencyModelEditDialogComponent;
  let fixture: ComponentFixture<CompetencyModelEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyModelEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyModelEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
