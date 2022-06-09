import { PayrollRequestReportCurrencyPipe } from './payroll-request-report-currency.pipe';

describe('PayrollRequestCurrencyPipe', () => {
  it('create an instance', () => {
    const pipe = new PayrollRequestReportCurrencyPipe(null);
    expect(pipe).toBeTruthy();
  });
});
