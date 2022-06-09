import { TestBed } from '@angular/core/testing';

import { FeedbackSetupDialogService } from './feedback-setup-dialog.service';

describe('FeedbackSetupDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FeedbackSetupDialogService = TestBed.get(FeedbackSetupDialogService);
    expect(service).toBeTruthy();
  });
});
