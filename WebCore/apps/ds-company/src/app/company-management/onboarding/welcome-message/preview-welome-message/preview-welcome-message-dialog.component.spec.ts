import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreviewWelcomeMessageDialogComponent } from './preview-welcome-message-dialog.component';

describe('PreviewWelcomeMessageDialogComponent', () => {
  let component: PreviewWelcomeMessageDialogComponent;
  let fixture: ComponentFixture<PreviewWelcomeMessageDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreviewWelcomeMessageDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreviewWelcomeMessageDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
