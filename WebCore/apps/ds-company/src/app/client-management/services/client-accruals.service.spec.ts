import { TestBed } from '@angular/core/testing';

import { ClientAccrualsService } from './client-accruals.service';

describe('ClientAccrualsService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClientAccrualsService = TestBed.get(ClientAccrualsService);
    expect(service).toBeTruthy();
  });
});
