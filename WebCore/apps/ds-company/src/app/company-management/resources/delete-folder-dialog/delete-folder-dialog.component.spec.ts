import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteFolderDialogComponent } from './delete-folder-dialog.component';

describe('DeleteFolderDialogComponent', () => {
  let component: DeleteFolderDialogComponent;
  let fixture: ComponentFixture<DeleteFolderDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeleteFolderDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeleteFolderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
