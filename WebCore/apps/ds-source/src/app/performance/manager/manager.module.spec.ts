import { PerformanceManagerModule } from './manager.module';

describe('ManagerModule', () => {
  let managerModule: PerformanceManagerModule;

  beforeEach(() => {
    managerModule = new PerformanceManagerModule();
  });

  it('should create an instance', () => {
    expect(managerModule).toBeTruthy();
  });
});
