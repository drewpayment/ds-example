
export interface ApproveHourSettings {
    clockEmployeeApproveHoursSettingsID: number;
    clientId: number;
    userId: number;
    hideNoActivity: boolean;
    hideGrandTotals: boolean;
    hideWeeklyTotals: boolean;
    hideActivity?: boolean;
    hideDailyTotals: boolean;
    showAllDays: boolean;
    defaultDaysFilter: number;
    employeesPerPage: number;
    payPeriod?: number;
}