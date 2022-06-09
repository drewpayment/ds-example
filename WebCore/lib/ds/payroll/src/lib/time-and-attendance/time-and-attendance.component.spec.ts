import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeAndAttendanceComponent } from './time-and-attendance.component';

describe('TimeAndAttendanceComponent', () => {
  let component: TimeAndAttendanceComponent;
  let fixture: ComponentFixture<TimeAndAttendanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeAndAttendanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeAndAttendanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
