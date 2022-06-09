import { TestBed } from '@angular/core/testing';

import { ReportScheduleSequenceDialogService } from './report-schedule-sequence-dialog.service';

describe('ReportScheduleSequenceDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReportScheduleSequenceDialogService = TestBed.get(ReportScheduleSequenceDialogService);
    expect(service).toBeTruthy();
  });
});
