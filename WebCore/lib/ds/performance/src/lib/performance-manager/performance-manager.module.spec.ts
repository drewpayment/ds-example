import { DsPerformanceManagerModule } from './performance-manager.module';

describe('PerformanceManagerModule', () => {
    let performanceManagerModule: DsPerformanceManagerModule;

    beforeEach(() => {
        performanceManagerModule = new DsPerformanceManagerModule();
    });

    it('should create an instance', () => {
        expect(performanceManagerModule).toBeTruthy();
    });
});
