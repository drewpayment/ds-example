import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeExitInterviewRequestComponent } from './employee-exit-interview-request.component';

describe('ExitInterviewRequestComponent', () => {
  let component: EmployeeExitInterviewRequestComponent;
  let fixture: ComponentFixture<EmployeeExitInterviewRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeExitInterviewRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeExitInterviewRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
 