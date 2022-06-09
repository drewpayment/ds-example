import { DsDialogModule } from './ds-dialog.module';

describe('DsDialogModule', () => {
  let dsDialogModule: DsDialogModule;

  beforeEach(() => {
    dsDialogModule = new DsDialogModule();
  });

  it('should create an instance', () => {
    expect(dsDialogModule).toBeTruthy();
  });
});
