import { CompanyAppModule } from './company.module';

describe('CompanyModule', () => {
  let companyModule: CompanyAppModule;

  beforeEach(() => {
    companyModule = new CompanyAppModule();
  });

  it('should create an instance', () => {
    expect(companyModule).toBeTruthy();
  });
});
