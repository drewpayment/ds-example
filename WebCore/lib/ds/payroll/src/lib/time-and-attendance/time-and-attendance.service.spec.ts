import { TestBed } from '@angular/core/testing';

import { TimeAndAttendanceService } from './time-and-attendance.service';

describe('TimeAndAttendanceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TimeAndAttendanceService = TestBed.get(TimeAndAttendanceService);
    expect(service).toBeTruthy();
  });
});
