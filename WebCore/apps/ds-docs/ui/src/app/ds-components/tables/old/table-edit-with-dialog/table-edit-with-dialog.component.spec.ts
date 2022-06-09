import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TableEditWithDialogComponent } from './table-edit-with-dialog.component';

describe('TableEditWithDialogComponent', () => {
  let component: TableEditWithDialogComponent;
  let fixture: ComponentFixture<TableEditWithDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TableEditWithDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TableEditWithDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
