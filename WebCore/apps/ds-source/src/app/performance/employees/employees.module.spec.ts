import { EmployeePerformanceAppModule } from './employees.module';

describe('EmployeesModule', () => {
  let employeesModule: EmployeePerformanceAppModule;

  beforeEach(() => {
    employeesModule = new EmployeePerformanceAppModule();
  });

  it('should create an instance', () => {
    expect(employeesModule).toBeTruthy();
  });
});
