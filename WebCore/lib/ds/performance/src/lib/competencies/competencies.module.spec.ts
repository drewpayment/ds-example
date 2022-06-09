import { CompetenciesModule } from './competencies.module';

describe('CompetenciesModule', () => {
  let competenciesModule: CompetenciesModule;

  beforeEach(() => {
    competenciesModule = new CompetenciesModule();
  });

  it('should create an instance', () => {
    expect(competenciesModule).toBeTruthy();
  });
});
