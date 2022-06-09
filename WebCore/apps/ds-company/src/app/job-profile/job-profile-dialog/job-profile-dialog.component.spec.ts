import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobProfileDialogComponent } from './job-profile-dialog.component';

describe('JobProfileDialogComponent', () => {
  let component: JobProfileDialogComponent;
  let fixture: ComponentFixture<JobProfileDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ JobProfileDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(JobProfileDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
