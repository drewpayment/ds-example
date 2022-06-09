import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NPSSurveyDialogComponent } from './nps-survey-dialog.component';

describe('NPSSurveyComponent', () => {
  let component: NPSSurveyDialogComponent;
  let fixture: ComponentFixture<NPSSurveyDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NPSSurveyDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NPSSurveyDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
