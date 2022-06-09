import { PerformanceSetupModule } from './setup.module';

describe('SetupModule', () => {
  let setupModule: PerformanceSetupModule;

  beforeEach(() => {
    setupModule = new PerformanceSetupModule();
  });

  it('should create an instance', () => {
    expect(setupModule).toBeTruthy();
  });
});
