import { TestBed, async, inject } from '@angular/core/testing';

import { PayrollRequestGuard } from './payroll-request.guard';

describe('PayrollRequestGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PayrollRequestGuard]
    });
  });

  it('should ...', inject([PayrollRequestGuard], (guard: PayrollRequestGuard) => {
    expect(guard).toBeTruthy();
  }));
});
