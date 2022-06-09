import { NgxDowngradesModule } from './ngx-downgrades.module';

describe('NgxDowngradesModule', () => {
  let ngxDowngradesModule: NgxDowngradesModule;

  beforeEach(() => {
    ngxDowngradesModule = new NgxDowngradesModule();
  });

  it('should create an instance', () => {
    expect(ngxDowngradesModule).toBeTruthy();
  });
});
