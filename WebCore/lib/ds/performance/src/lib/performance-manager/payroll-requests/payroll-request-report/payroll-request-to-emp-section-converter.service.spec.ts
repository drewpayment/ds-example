import { TestBed } from '@angular/core/testing';

import { PayrollRequestToEmpSectionConverterService } from './payroll-request-to-emp-section-converter.service';

describe('PayrollRequestToEmpSectionConverterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PayrollRequestToEmpSectionConverterService = TestBed.get(PayrollRequestToEmpSectionConverterService);
    expect(service).toBeTruthy();
  });
});
