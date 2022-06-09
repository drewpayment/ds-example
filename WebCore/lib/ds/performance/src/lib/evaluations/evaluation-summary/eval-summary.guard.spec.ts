import { TestBed, async, inject } from '@angular/core/testing';

import { EvalSummaryGuard } from './eval-summary.guard';

describe('EvalSummaryGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EvalSummaryGuard]
    });
  });

  it('should ...', inject([EvalSummaryGuard], (guard: EvalSummaryGuard) => {
    expect(guard).toBeTruthy();
  }));
});
