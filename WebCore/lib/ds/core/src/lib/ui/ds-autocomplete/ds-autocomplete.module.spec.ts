import { DsAutocompleteModule } from './ds-autocomplete.module';

describe('DsAutocompleteModule', () => {
  let dsAutocompleteModule: DsAutocompleteModule;

  beforeEach(() => {
    dsAutocompleteModule = new DsAutocompleteModule();
  });

  it('should create an instance', () => {
    expect(dsAutocompleteModule).toBeTruthy();
  });
});
