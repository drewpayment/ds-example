import { TestBed } from '@angular/core/testing';

import { PayrollRequestService } from './payroll-request.service';

describe('PayrollRequestService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PayrollRequestService = TestBed.get(PayrollRequestService);
    expect(service).toBeTruthy();
  });
});
