import { DsAppConfigModule } from './app-config.module';

describe('AppConfigModule', () => {
  let appConfigModule: DsAppConfigModule;

  beforeEach(() => {
    appConfigModule = new DsAppConfigModule();
  });

  it('should create an instance', () => {
    expect(appConfigModule).toBeTruthy();
  });
});
