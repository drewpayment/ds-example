import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewFinalDisclaimerDialogComponent } from './preview-final-disclaimer-dialog.component';

describe('PreviewFinalDisclaimerDialogComponent', () => {
  let component: PreviewFinalDisclaimerDialogComponent;
  let fixture: ComponentFixture<PreviewFinalDisclaimerDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreviewFinalDisclaimerDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewFinalDisclaimerDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
