import { DsCoreLocationModule } from './location.module';

describe('LocationModule', () => {
  let locationModule: DsCoreLocationModule;

  beforeEach(() => {
    locationModule = new DsCoreLocationModule();
  });

  it('should create an instance', () => {
    expect(locationModule).toBeTruthy();
  });
});
