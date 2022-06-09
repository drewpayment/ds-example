import { DsProgressModule } from './ds-progress.module';

describe('DsProgressModule', () => {
  let dsProgressModule: DsProgressModule;

  beforeEach(() => {
    dsProgressModule = new DsProgressModule();
  });

  it('should create an instance', () => {
    expect(dsProgressModule).toBeTruthy();
  });
});
