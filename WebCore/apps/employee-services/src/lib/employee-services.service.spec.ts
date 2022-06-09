import { TestBed } from '@angular/core/testing';

import { EmployeeServicesService } from './employee-services.service';

describe('EmployeeServicesService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: EmployeeServicesService = TestBed.get(EmployeeServicesService);
    expect(service).toBeTruthy();
  });
});
