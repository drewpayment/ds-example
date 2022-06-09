const SystemAdmin = "SystemAdmin";

export const ActionTypes = {
  SystemAdmin: {
    ReadWriteAutomaticBilling: `${SystemAdmin}.ReadWriteAutomaticBilling`,
  },
  EmployeeNavigator: {
    ViewReports: `EmployeeNavigator.ViewReports`,
    SyncData: `EmployeeNavigator.SyncData`,
  },
  Payroll: {
    PayrollSystemAdministrator: `Payroll.PayrollSystemAdministrator`,
    ReadPayrollHistory: `Payroll.ReadPayrollHistory`,
  },
  Features: {
    PerformanceReviews: 'Feature.PerformanceReviews',
  },
  GeneralLedger: {
    GeneralLedgerAdministrator: 'GeneralLedger.GeneralLedgerWrite'
  }
};
