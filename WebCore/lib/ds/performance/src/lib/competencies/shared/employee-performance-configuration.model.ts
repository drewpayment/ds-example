import { IOneTimeEarningSettings } from '../shared/onetime-earning-settings';
export interface EmployeePerformanceConfiguration {
    competencyModelId: number,
    employeeId: number,
    hasAdditionalEarnings: boolean,
    employee: {
        employeeId: number,
        firstName: string,
        lastName: string
    },
    oneTimeEarningSettings: IOneTimeEarningSettings,
    clientId: number
}
