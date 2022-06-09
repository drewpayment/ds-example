import { TestBed } from '@angular/core/testing';

import { CheckStockService } from './check-stock.service';

describe('CheckStockService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CheckStockService = TestBed.get(CheckStockService);
    expect(service).toBeTruthy();
  });
});
