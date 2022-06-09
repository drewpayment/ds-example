import { TestBed } from '@angular/core/testing';

import { ApprovalProcessSummaryDialogService } from './approval-process-summary-dialog.service';

describe('ApprovalProcessSummaryDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ApprovalProcessSummaryDialogService = TestBed.get(ApprovalProcessSummaryDialogService);
    expect(service).toBeTruthy();
  });
});
