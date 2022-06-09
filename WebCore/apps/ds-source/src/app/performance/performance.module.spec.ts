import { PerformanceAppModule } from './performance.module';

describe('PerformanceAppModule', () => {
  let performanceModule: PerformanceAppModule;

  beforeEach(() => {
    performanceModule = new PerformanceAppModule();
  });

  it('should create an instance', () => {
    expect(performanceModule).toBeTruthy();
  });
});
