import { TestBed } from '@angular/core/testing';

import { ClientStatisticsApiService } from './client-statistics-api.service';

describe('ClientStatisticsApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClientStatisticsApiService = TestBed.get(ClientStatisticsApiService);
    expect(service).toBeTruthy();
  });
});
