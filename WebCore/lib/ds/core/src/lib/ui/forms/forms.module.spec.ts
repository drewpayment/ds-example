import { DsCoreFormsModule } from './forms.module';

describe('FormsModule', () => {
  let formsModule: DsCoreFormsModule;

  beforeEach(() => {
    formsModule = new DsCoreFormsModule();
  });

  it('should create an instance', () => {
    expect(formsModule).toBeTruthy();
  });
});
