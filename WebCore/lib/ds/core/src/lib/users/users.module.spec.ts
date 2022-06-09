import { DsUsersModule } from './users.module';

describe('UsersModule', () => {
  let usersModule: DsUsersModule;

  beforeEach(() => {
    usersModule = new DsUsersModule();
  });

  it('should create an instance', () => {
    expect(usersModule).toBeTruthy();
  });
});
