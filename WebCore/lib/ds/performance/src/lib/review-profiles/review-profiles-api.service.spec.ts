import { TestBed } from '@angular/core/testing';

import { ReviewProfilesApiService } from './review-profiles-api.service';

describe('ReviewProfilesApiService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewProfilesApiService = TestBed.get(ReviewProfilesApiService);
    expect(service).toBeTruthy();
  });
});
