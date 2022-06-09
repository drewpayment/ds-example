import { TestBed } from '@angular/core/testing';

import { PayrollRequestReportStoreService } from './payroll-request-report-store.service';

describe('PayrollRequestReportStoreService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PayrollRequestReportStoreService = TestBed.get(PayrollRequestReportStoreService);
    expect(service).toBeTruthy();
  });
});
