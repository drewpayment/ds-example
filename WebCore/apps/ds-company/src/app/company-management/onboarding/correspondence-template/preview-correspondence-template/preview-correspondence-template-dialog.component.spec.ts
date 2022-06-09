import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewCorrespondenceTemplateDialogComponent } from './preview-correspondence-template-dialog.component';

describe('PreviewCorrespondenceTemplateDialogComponent', () => {
  let component: PreviewCorrespondenceTemplateDialogComponent;
  let fixture: ComponentFixture<PreviewCorrespondenceTemplateDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreviewCorrespondenceTemplateDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewCorrespondenceTemplateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
