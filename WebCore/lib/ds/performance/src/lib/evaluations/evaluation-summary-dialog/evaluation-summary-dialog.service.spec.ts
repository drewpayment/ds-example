import { TestBed } from '@angular/core/testing';

import { EvaluationSummaryDialogService } from './evaluation-summary-dialog.service';

describe('EvaluationSummaryDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EvaluationSummaryDialogService = TestBed.get(EvaluationSummaryDialogService);
    expect(service).toBeTruthy();
  });
});
