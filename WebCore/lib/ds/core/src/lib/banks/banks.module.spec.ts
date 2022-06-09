import { BanksModule } from './banks.module';

describe('BanksModule', () => {
  let banksModule: BanksModule;

  beforeEach(() => {
    banksModule = new BanksModule();
  });

  it('should create an instance', () => {
    expect(banksModule).toBeTruthy();
  });
});
