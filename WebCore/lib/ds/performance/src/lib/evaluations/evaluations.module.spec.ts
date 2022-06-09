import { DsEvaluationsModule } from './evaluations.module';

describe('EvaluationsModule', () => {
    let evaluationsModule: DsEvaluationsModule;

    beforeEach(() => {
        evaluationsModule = new DsEvaluationsModule();
    });

    it('should create an instance', () => {
        expect(evaluationsModule).toBeTruthy();
    });
});
