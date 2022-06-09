import { AjsUpgradesModule } from './ajs-upgrades.module';

describe('AjsUpgradesModule', () => {
  let ajsUpgradesModule: AjsUpgradesModule;

  beforeEach(() => {
    ajsUpgradesModule = new AjsUpgradesModule();
  });

  it('should create an instance', () => {
    expect(ajsUpgradesModule).toBeTruthy();
  });
});
