import { TestBed } from '@angular/core/testing';

import { PaycheckTableService } from './paycheck-table.service';

describe('PaycheckTableService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PaycheckTableService = TestBed.get(PaycheckTableService);
    expect(service).toBeTruthy();
  });
});
