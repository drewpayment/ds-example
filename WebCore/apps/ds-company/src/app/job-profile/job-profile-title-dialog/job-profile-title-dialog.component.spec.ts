import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobProfileTitleDialogComponent } from './job-profile-title-dialog.component';

describe('JobProfileTitleDialogComponent', () => {
  let component: JobProfileTitleDialogComponent;
  let fixture: ComponentFixture<JobProfileTitleDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobProfileTitleDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JobProfileTitleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
