import { TestBed } from '@angular/core/testing';

import { ReviewSummaryDialogService } from './review-summary-dialog.service';

describe('ReviewSummaryDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewSummaryDialogService = TestBed.get(ReviewSummaryDialogService);
    expect(service).toBeTruthy();
  });
});
