import { TestBed } from '@angular/core/testing';

import { ReviewPolicyService } from './review-policy.service';

describe('ReviewPolicyService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ReviewPolicyService = TestBed.get(ReviewPolicyService);
    expect(service).toBeTruthy();
  });
});
