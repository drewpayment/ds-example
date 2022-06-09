import { DsPerformanceReviewProfilesModule } from './review-profiles.module';

describe('ReviewProfilesModule', () => {
  let reviewProfilesModule: DsPerformanceReviewProfilesModule;

  beforeEach(() => {
    reviewProfilesModule = new DsPerformanceReviewProfilesModule();
  });

  it('should create an instance', () => {
    expect(reviewProfilesModule).toBeTruthy();
  });
});
