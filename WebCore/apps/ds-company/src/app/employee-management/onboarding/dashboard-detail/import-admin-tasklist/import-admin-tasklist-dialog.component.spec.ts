import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportAdminTasklistDialogComponent } from './import-admin-tasklist-dialog.component;

describe('ImportAdminTasklistComponent', () => {
  let component: ImportAdminTasklistDialogComponent;
  let fixture: ComponentFixture<ImportAdminTasklistDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportAdminTasklistDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportAdminTasklistDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
