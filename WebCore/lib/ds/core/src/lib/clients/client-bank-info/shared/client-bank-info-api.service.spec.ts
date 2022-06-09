import { TestBed } from '@angular/core/testing';

import { ClientBankInfoApiService } from './client-bank-info-api.service';

describe('ClientBankInfoApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClientBankInfoApiService = TestBed.get(ClientBankInfoApiService);
    expect(service).toBeTruthy();
  });
});
