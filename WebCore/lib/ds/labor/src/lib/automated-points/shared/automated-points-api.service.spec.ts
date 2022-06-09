import { TestBed } from '@angular/core/testing';

import { AutomatedPointsApiService } from './automated-points-api.service';

describe('AutomatedPointsApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AutomatedPointsApiService = TestBed.get(AutomatedPointsApiService);
    expect(service).toBeTruthy();
  });
});
