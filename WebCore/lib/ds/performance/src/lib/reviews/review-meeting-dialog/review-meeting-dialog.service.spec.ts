import { TestBed } from '@angular/core/testing';

import { ReviewMeetingDialogService } from './review-meeting-dialog.service';

describe('ReviewMeetingDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewMeetingDialogService = TestBed.get(ReviewMeetingDialogService);
    expect(service).toBeTruthy();
  });
});
