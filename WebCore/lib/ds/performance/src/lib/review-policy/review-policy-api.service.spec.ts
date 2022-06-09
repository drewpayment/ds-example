import { TestBed } from '@angular/core/testing';

import { ReviewPolicyApiService } from './review-policy-api.service';

describe('ReviewPolicyApiService', () => {
    beforeEach(() => TestBed.configureTestingModule({}));

    it('should be created', () => {
        const service: ReviewPolicyApiService = TestBed.get(ReviewPolicyApiService);
        expect(service).toBeTruthy();
    });
});
