import { TestBed } from '@angular/core/testing';

import { UpdateBalanceDialogService } from './update-balance-dialog.service';

describe('UpdateBalanceDialogService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UpdateBalanceDialogService = TestBed.get(UpdateBalanceDialogService);
    expect(service).toBeTruthy();
  });
});
