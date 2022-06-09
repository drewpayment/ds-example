import { GoalsModule } from './goals.module';

describe('GoalsModule', () => {
  let goalsModule: GoalsModule;

  beforeEach(() => {
    goalsModule = new GoalsModule();
  });

  it('should create an instance', () => {
    expect(goalsModule).toBeTruthy();
  });
});
