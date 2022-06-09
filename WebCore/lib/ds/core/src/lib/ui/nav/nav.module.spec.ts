import { DsUiNavModule } from './nav.module';

describe('DsUiNavModule', () => {
  let navModule: DsUiNavModule;

  beforeEach(() => {
    navModule = new DsUiNavModule();
  });

  it('should create an instance', () => {
    expect(navModule).toBeTruthy();
  });
});
