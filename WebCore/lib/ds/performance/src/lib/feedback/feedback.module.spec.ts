import { DsPerformanceFeedbackModule } from './feedback.module';

describe('FeedbackModule', () => {
    let feedbackModule: DsPerformanceFeedbackModule;

    beforeEach(() => {
        feedbackModule = new DsPerformanceFeedbackModule();
    });

    it('should create an instance', () => {
        expect(feedbackModule).toBeTruthy();
    });
});
