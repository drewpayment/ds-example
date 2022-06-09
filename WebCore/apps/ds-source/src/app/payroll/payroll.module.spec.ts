import { PayrollAppModule } from './payroll.module';

describe('PayrollModule', () => {
  let payrollModule: PayrollAppModule;

  beforeEach(() => {
    payrollModule = new PayrollAppModule();
  });

  it('should create an instance', () => {
    expect(payrollModule).toBeTruthy();
  });
});
