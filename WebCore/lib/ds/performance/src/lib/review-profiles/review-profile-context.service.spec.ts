import { TestBed } from '@angular/core/testing';

import { ReviewProfileContextService } from './review-profile-context.service';

describe('ReviewProfileContextService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewProfileContextService = TestBed.get(ReviewProfileContextService);
    expect(service).toBeTruthy();
  });
});
