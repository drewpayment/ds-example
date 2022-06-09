import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { JobProfileModalComponent } from './job-profile-modal.component';

describe('JobProfileModalComponent', () => {
  let component: JobProfileModalComponent;
  let fixture: ComponentFixture<JobProfileModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ JobProfileModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(JobProfileModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
