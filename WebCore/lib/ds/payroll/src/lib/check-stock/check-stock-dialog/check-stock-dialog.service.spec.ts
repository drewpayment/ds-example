import { TestBed } from '@angular/core/testing';

import { CheckStockDialogService } from './check-stock-dialog.service';

describe('CheckStockDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: CheckStockDialogService = TestBed.get(CheckStockDialogService);
    expect(service).toBeTruthy();
  });
});
