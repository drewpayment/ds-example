import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAttachmentFolderDialogComponent } from './add-attachment-folder-dialog.component';

describe('AddAttachmentFolderDialogComponent', () => {
  let component: AddAttachmentFolderDialogComponent;
  let fixture: ComponentFixture<AddAttachmentFolderDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAttachmentFolderDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAttachmentFolderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
