import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompetencyEditDialogComponent } from './competency-edit-dialog.component';

describe('CompetencyEditDialogComponent', () => {
  let component: CompetencyEditDialogComponent;
  let fixture: ComponentFixture<CompetencyEditDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompetencyEditDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompetencyEditDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
