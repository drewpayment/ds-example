import { TestBed } from '@angular/core/testing';

import { BanksApiService } from './banks-api.service';

describe('BanksApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BanksApiService = TestBed.get(BanksApiService);
    expect(service).toBeTruthy();
  });
});
