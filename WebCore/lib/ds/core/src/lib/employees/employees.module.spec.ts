import { DsCoreEmployeesModule } from './employees.module';

describe('EmployeesModule', () => {
    let employeesModule: DsCoreEmployeesModule;

    beforeEach(() => {
        employeesModule = new DsCoreEmployeesModule();
    });

    it('should create an instance', () => {
        expect(employeesModule).toBeTruthy();
    });
});
