import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEmployeeAttachmentDialogComponent } from './add-attachment-dialog.component';

describe('AddEmployeeAttachmentDialogComponent', () => {
  let component: AddEmployeeAttachmentDialogComponent;
  let fixture: ComponentFixture<AddEmployeeAttachmentDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEmployeeAttachmentDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEmployeeAttachmentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
