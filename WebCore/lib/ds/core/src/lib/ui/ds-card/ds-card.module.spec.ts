import { DsCardModule } from './ds-card.module';

describe('DsCardModule', () => {
  let dsCardModule: DsCardModule;

  beforeEach(() => {
    dsCardModule = new DsCardModule();
  });

  it('should create an instance', () => {
    expect(dsCardModule).toBeTruthy();
  });
});
