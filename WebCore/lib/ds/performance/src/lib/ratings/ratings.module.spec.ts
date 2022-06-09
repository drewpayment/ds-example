import { RatingsModule } from './ratings.module';

describe('RatingsModule', () => {
  let ratingsModule: RatingsModule;

  beforeEach(() => {
    ratingsModule = new RatingsModule();
  });

  it('should create an instance', () => {
    expect(ratingsModule).toBeTruthy();
  });
});
