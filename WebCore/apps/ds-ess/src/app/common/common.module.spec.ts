import { EssCommonModule } from './common.module';

describe('CommonModule', () => {
  let commonModule: EssCommonModule;

  beforeEach(() => {
    commonModule = new EssCommonModule();
  });

  it('should create an instance', () => {
    expect(commonModule).toBeTruthy();
  });
});
