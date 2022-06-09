import { DsExpansionModule } from './ds-expansion.module';

describe('DsExpansionModule', () => {
  let dsExpansionModule: DsExpansionModule;

  beforeEach(() => {
    dsExpansionModule = new DsExpansionModule();
  });

  it('should create an instance', () => {
    expect(dsExpansionModule).toBeTruthy();
  });
});
