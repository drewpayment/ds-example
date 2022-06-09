import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeAttachmentViewerComponent } from './employee-attachment-viewer.component';

describe('EmployeeAttachmentViewerComponent', () => {
  let component: EmployeeAttachmentViewerComponent;
  let fixture: ComponentFixture<EmployeeAttachmentViewerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeAttachmentViewerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeAttachmentViewerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
