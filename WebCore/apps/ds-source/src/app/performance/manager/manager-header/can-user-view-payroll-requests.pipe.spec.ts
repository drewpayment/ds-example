import { CanUserViewPayrollRequestsPipe } from './can-user-view-payroll-requests.pipe';

describe('CanUserViewPayrollRequestsPipe', () => {
  it('create an instance', () => {
    const pipe = new CanUserViewPayrollRequestsPipe();
    expect(pipe).toBeTruthy();
  });
});
