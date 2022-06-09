import { CompanyJobProfilesModule } from './job-profiles.module';

describe('JobProfilesModule', () => {
  let jobProfilesModule: CompanyJobProfilesModule;

  beforeEach(() => {
    jobProfilesModule = new CompanyJobProfilesModule();
  });

  it('should create an instance', () => {
    expect(jobProfilesModule).toBeTruthy();
  });
});
