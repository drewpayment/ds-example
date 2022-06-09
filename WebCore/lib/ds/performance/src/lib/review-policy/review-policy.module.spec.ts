import { DsPerformanceReviewPolicyModule } from './review-policy.module';

describe('ReviewCyclesModule', () => {
    let reviewPolicysModule: DsPerformanceReviewPolicyModule;

    beforeEach(() => {
        reviewPolicysModule = new DsPerformanceReviewPolicyModule();
    });

    it('should create an instance', () => {
        expect(reviewPolicysModule).toBeTruthy();
    });
});
