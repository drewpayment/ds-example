import { DateTimeModule } from './datetime.module';

describe('DatetimeModule', () => {
  let datetimeModule: DateTimeModule;

  beforeEach(() => {
    datetimeModule = new DateTimeModule();
  });

  it('should create an instance', () => {
    expect(datetimeModule).toBeTruthy();
  });
});
