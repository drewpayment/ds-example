import { TestBed } from '@angular/core/testing';

import { AppConfigApiService } from './app-config-api.service';

describe('AppConfigApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AppConfigApiService = TestBed.get(AppConfigApiService);
    expect(service).toBeTruthy();
  });
});
