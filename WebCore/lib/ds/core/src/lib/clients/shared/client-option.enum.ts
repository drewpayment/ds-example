import { isInstanceOfClientOptionKey } from "./index";

/**
 * When adding new enum value,
 * make sure to add the corresponding string to @see ALL_CLIENT_OPTIONS !
 * 
 * Corresponds to server-side enum: `Dominion.Core.Dto.Misc.AccountOption`
 * and all values of `select ClientOptionControlId from dbo.ClientOptionControls`.
 */
export enum ClientOption {
    Reporting_HideRoutingOnMasterControl = 1, 

    /// <summary>
    /// #02 | Selects what employees will be picked by default in the Payroll application.
    /// </summary>
    Payroll_DefaultEmployeesSelected = 2, 

    /// <summary>
    /// #03 | Selects what employees will be picked by default in the TimeClock application.
    /// </summary>
    TimeClock_DefaultEmployeesSelected = 3, 

    /// <summary>
    /// #04 | Includes salaried employee's hours in the import.
    /// </summary>
    Payroll_IncludeSalaryHoursInImport = 4, 

    /// <summary>
    /// #05 | How long before a user's password expires.
    /// </summary>
    Timeout_DaysForPasswordExpiration = 5, 

    /// <summary>
    /// #06 | List of IP Addresses employee's can use the system to punch from.
    /// </summary>
    TimeClock_IPSecurity = 6, 

    /// <summary>
    /// #07 | Determines which time interval (Monthly, Weekly, etc) will be displayed by default in the punch calendar.
    /// </summary>
    TimeClock_DefaultPunchCalendarView = 7, 

    /// <summary>
    /// #08 | Populates payroll totals on import.
    /// </summary>
    Payroll_PopulatePayrollTotalsOnImport = 8, 

    /// <summary>
    /// #09 | Determines which punches to show to employees (None, Rounded, Actual, etc).
    /// </summary>
    TimeClock_PunchesToShowToEmployees = 9, 

    /// <summary>
    /// #10 | Show punch exceptions warning on import.
    /// </summary>
    TimeClock_ShowPunchExceptionsWarningOnImport = 10, 

    /// <summary>
    /// #11 | Determines which hours require approval.
    /// </summary>
    TimeClock_ApprovalOptions = 11, 

    /// <summary>
    /// #12 | Apply tips catch up to match minimum wage.
    /// </summary>
    Payroll_IncreaseTipsToMatchWage = 12, 

    /// <summary>
    /// #13 | Block supervisor from authorizing timecards.
    /// </summary>
    TimeClock_BlockSupervisorFromAuthorizingTimecards = 13, 

    /// <summary>
    /// #14 | Show check history only on check date or later.
    /// </summary>
    Payroll_CheckHistoryOnCheckDateOrLater = 14, 

    /// <summary>
    /// #15 | Automatic vs manual shift options.
    /// </summary>
    TimeClock_UseShifts = 15, 

    /// <summary>
    /// #16 | Require employees to select a cost center.
    /// </summary>
    TimeClock_RequireEmployeeToPickCostCenter = 16, 

    /// <summary>
    /// #17 | Number of days to show on punch screen.
    /// </summary>
    TimeClock_DaysToShowOnPunch = 17, 

    // Payroll_W2ConsentType                                 = 18, <-- Not present in database

    /// <summary>
    /// #19 | Determines what type of users can edit company schedules.
    /// </summary>
    TimeClock_UsersAllowedToEditSchedules = 19, 

    /// <summary>
    /// #20 | Determines if the client or Dominion will print W2's.
    /// </summary>
    Payroll_WhoWillPrintW2s = 20, 

    /// <summary>
    /// #21 | Display preview warning for "Home Cost Center not setup."
    /// </summary>
    Payroll_PreviewCostCenterWarning = 21, 

    /// <summary>
    /// #22 | Include adjustments in employee check history.
    /// </summary>
    Payroll_IncludeAdjustmentsEmployeeCheckHistory = 22, 

    /// <summary>
    /// #23 | Include weekends default.
    /// </summary>
    TimeClock_IncludeWeekendsDefault = 23, 

    // AllowAdjustmentImports                                = 24, <-- Not present in database

    /// <summary>
    /// #25 | Password protect employee payroll stubs.
    /// </summary>
    Payroll_PasswordProtectPaystubs = 25, 

    /// <summary>
    /// #26 | Average hours on master control.
    /// </summary>
    Reporting_AvgHoursOnMasterControl = 26, 

    /// <summary>
    /// #27 | Number of days employees have ESS access.
    /// </summary>
    Payroll_NumberDaysEmployeeHasEssAccess = 27, 

    /// <summary>
    /// #28 | Allow client admins to print remote manual checks.
    /// </summary>
    Payroll_AllowCompanyAdminToPrintManualChecks = 28, 

    /// <summary>
    /// #29 | Determines how to set an employee's status upon seperation (None, LastPay, Terminated, etc).
    /// </summary>
    Payroll_AutomaticallyTerminateOnSeparationDate = 29, 

    /// <summary>
    /// #30 | Leave management employee view only.
    /// </summary>
    TimeClock_EmployeeViewOnlyLeaveManagement = 30, 

    /// <summary>
    /// #31 | Split salary based on grid hours.
    /// </summary>
    Payroll_SplitSalaryBasedOnGridHours = 31, 

    /// <summary>
    /// #32 | Determines how employee's will be organized by default (Name, Number, etc).
    /// </summary>
    Payroll_DefaultEmployeeSequence = 32, 

    /// <summary>
    /// #33 | Enable temp agency billing.
    /// </summary>
    Payroll_EnableTempAgencyBilling = 33, 

    /// <summary>
    /// #34 | Enable scheduled reports.
    /// </summary>
    Reporting_EnableScheduledReportsOption = 34, 

    /// <summary>
    /// #35 | Exclude temps.
    /// </summary>
    Payroll_ExcludeTemps = 35, 

    /// <summary>
    /// #36 | Apply departments accross all divisions.
    /// </summary>
    Payroll_DepartmentsAcrossAllDivisions = 36, 

    /// <summary>
    /// #37 | Allow income tax exempt.
    /// </summary>
    Payroll_AllowIncomeWageExempt = 37, 

    /// <summary>
    /// #38 | Show hours to the hundreths place (ie #.##)
    /// </summary>
    TimeClock_TimeShowHoursInHundreths = 38, 

    // HideInactiveDeductions                                = 39, <-- Not present in database

    /// <summary>
    /// #40 | Enable customized report owner option.
    /// </summary>
    Reporting_EnableCustomizedReportOwnerOption = 40, 

    /// <summary>
    /// #41 | Lock current timecards.
    /// </summary>
    TimeClock_LockTimecards = 41, 

    /// <summary>
    /// #42 | Lock closed payrolls.
    /// </summary>
    TimeClock_LockClosedPayrolls = 42, 

    /// <summary>
    /// #43 | Stop direct deposit on last pay.
    /// </summary>
    Payroll_StopDirectDepositonLastPay = 43, 

    /// <summary>
    /// #44 | Allow employees to request changes to their tax status.
    /// </summary>
    Payroll_AllowEmployeeTaxStatusChangeRequests = 44, 

    /// <summary>
    /// #45 | Disable employee search ability.
    /// </summary>
    Payroll_DisableEmployeeListDropdownListInEmployeeHeader = 45, 

    /// <summary>
    /// #46 | Supervisors can enable employees.
    /// </summary>
    Payroll_SupervisorsCanEnableEmployees = 46, 

    /// <summary>
    /// #47 | Show clock dropdown list.
    /// </summary>
    TimeClock_ShowClockDropdownList = 47, 

    /// <summary>
    /// #48 | Catchup SUTA / State FUTA.
    /// </summary>
    Payroll_CatchupSutaAndStateFuta = 48, 

    /// <summary>
    /// #49 | Split holiday among shifts.
    /// </summary>
    TimeClock_SplitHolidaysAmongShifts = 49, 

    /// <summary>
    /// #50 | Show subcheck on benefit screen.
    /// </summary>
    TimeClock_ShowSubcheckOnBenefitScreen = 50, 

    /// <summary>
    /// #51 | Timeout in minutes for Company Admin users.
    /// </summary>
    Timeout_CompanyAdminLockoutMinutes = 51, 

    /// <summary>
    /// #52 | Timeout in minutes for Supervisor users.
    /// </summary>
    Timeout_SupervisorLockoutMinutes = 52, 

    /// <summary>
    /// #53 | Timeout in minutes for Employee users.
    /// </summary>
    Timeout_EmployeeLockoutMinutes = 53, 

    /// <summary>
    /// #54 | Determines type of hours (Regular, Regular+Overtime+Double, etc).
    /// </summary>
    TimeClock_TypeOfHours = 54, 

    /// <summary>
    /// #55 | Show split shifts on timecard authorization.
    /// </summary>
    TimeClock_ShowSplitShifts = 55, 

    /// <summary>
    /// #56 | Allow deductions to be added by code.
    /// </summary>
    Payroll_AddDeductionByCode = 56, 

    /// <summary>
    /// #57 | Allow multiple file pay data imports.
    /// </summary>
    Payroll_ImportMultipleFiles = 57, 

    /// <summary>
    /// #58 | Show cost center details for timecard authorization.
    /// </summary>
    TimeClock_ShowCostCenterTooltip = 58, 

    /// <summary>
    /// #59 | Allow pre-overtime allocation.
    /// </summary>
    TimeClock_AllowPreOverTimeAllocation = 59, 

    /// <summary>
    /// #60 | Determines the default one-time deduction import option (Add, Replace, Force, etc).
    /// </summary>
    Payroll_DefaultOneTimeDeductionImportOption = 60, 

    /// <summary>
    /// #61 | Run labor source reports in hundreths (ie #.##).
    /// </summary>
    Reporting_RunLaborSourceReportsInHundredths = 61, 

    /// <summary>
    /// #62 | Clock punches 0 fill work code to XX spaces (eg 0432).
    /// </summary>
    TimeClock_ZeroFillWorkCode = 62, 

    /// <summary>
    /// #63 | Show rate override on benefit screen.
    /// </summary>
    TimeClock_ShowRateOverrideOnBenefits = 63, 

    /// <summary>
    /// #64 | Allow editing of notes on locked time cards.
    /// </summary>
    TimeClock_AllowEditOfNotesOnLockedTimeCard = 64, 

    /// <summary>
    /// #65 | Allow editing of schedules on loced time cards.
    /// </summary>
    TimeClock_AllowEditOfSchedulesOnLockedTimeCard = 65,

    /// <summary>
    /// #66 | Allow Time Off Requests from Punch Calendar.
    /// </summary>
    TimeClock_AllowTimeOffRequestsFromPunchCalender = 66,

    /// <summary>
    /// #67 | Make poinst reports visible to employees.
    /// </summary>
    Reporting_PointsReportsVisibleToEmployees = 67,

    /// <summary>
    /// #68 | Determines who will print ACA reports (Dominion, Client, etc)
    /// </summary>
    Reporting_WhoWillPrintAcaReports = 68,

    /// <summary>
    /// #69 | Determines if the company admin has the ability to edit a locked time card
    /// </summary>
    TimeClock_CompanyAdminCanEditLockedTimeCards = 69,

    Payroll_ShowVoidCheck = 70,

    /// <summary>
    /// #71 | Mask Social Security Number
    /// </summary>
    Reporting_MaskSocialSecurityNumber = 71,

    /// <summary>
    /// #72 Client can use employee import
    /// </summary>
    Payroll_EmployeeImport = 72,
    
    /// <summary>
    /// #73 | Length of ipad pin number
    /// </summary>
    TimeClock_IPadPinNumberLength = 73,

    /// <summary>
    /// #74 | Paycheck logo position
    /// </summary>
    Reporting_PaycheckLogoPosition = 74,
}

/**
 * Used to facillitate user-defined type-guard
 * @see isInstanceOfClientOptionKey
 */
export const ALL_CLIENT_OPTIONS = [
    'Reporting_HideRoutingOnMasterControl',
    'Payroll_DefaultEmployeesSelected',
    'TimeClock_DefaultEmployeesSelected',
    'Payroll_IncludeSalaryHoursInImport',
    'Timeout_DaysForPasswordExpiration',
    'TimeClock_IPSecurity',
    'TimeClock_DefaultPunchCalendarView',
    'Payroll_PopulatePayrollTotalsOnImport',
    'TimeClock_PunchesToShowToEmployees',
    'TimeClock_ShowPunchExceptionsWarningOnImport',
    'TimeClock_ApprovalOptions',
    'Payroll_IncreaseTipsToMatchWage',
    'TimeClock_BlockSupervisorFromAuthorizingTimecards',
    'Payroll_CheckHistoryOnCheckDateOrLater',
    'TimeClock_UseShifts',
    'TimeClock_RequireEmployeeToPickCostCenter',
    'TimeClock_DaysToShowOnPunch',
    'Payroll_W2ConsentType',
    'TimeClock_UsersAllowedToEditSchedules',
    'Payroll_WhoWillPrintW2s',
    'Payroll_PreviewCostCenterWarning',
    'Payroll_IncludeAdjustmentsEmployeeCheckHistory',
    'TimeClock_IncludeWeekendsDefault',
    'AllowAdjustmentImports',
    'Payroll_PasswordProtectPaystubs',
    'Reporting_AvgHoursOnMasterControl',
    'Payroll_NumberDaysEmployeeHasEssAccess',
    'Payroll_AllowCompanyAdminToPrintManualChecks',
    'Payroll_AutomaticallyTerminateOnSeparationDate',
    'TimeClock_EmployeeViewOnlyLeaveManagement',
    'Payroll_SplitSalaryBasedOnGridHours',
    'Payroll_DefaultEmployeeSequence',
    'Payroll_EnableTempAgencyBilling',
    'Reporting_EnableScheduledReportsOption',
    'Payroll_ExcludeTemps',
    'Payroll_DepartmentsAcrossAllDivisions',
    'Payroll_AllowIncomeWageExempt',
    'TimeClock_TimeShowHoursInHundreths',
    'HideInactiveDeductions',
    'Reporting_EnableCustomizedReportOwnerOption',
    'TimeClock_LockTimecards',
    'TimeClock_LockClosedPayrolls',
    'Payroll_StopDirectDepositonLastPay',
    'Payroll_AllowEmployeeTaxStatusChangeRequests',
    'Payroll_DisableEmployeeListDropdownListInEmployeeHeader',
    'Payroll_SupervisorsCanEnableEmployees',
    'TimeClock_ShowClockDropdownList',
    'Payroll_CatchupSutaAndStateFuta',
    'TimeClock_SplitHolidaysAmongShifts',
    'TimeClock_ShowSubcheckOnBenefitScreen',
    'Timeout_CompanyAdminLockoutMinutes',
    'Timeout_SupervisorLockoutMinutes',
    'Timeout_EmployeeLockoutMinutes',
    'TimeClock_TypeOfHours',
    'TimeClock_ShowSplitShifts',
    'Payroll_AddDeductionByCode',
    'Payroll_ImportMultipleFiles',
    'TimeClock_ShowCostCenterTooltip',
    'TimeClock_AllowPreOverTimeAllocation',
    'Payroll_DefaultOneTimeDeductionImportOption',
    'Reporting_RunLaborSourceReportsInHundredths',
    'TimeClock_ZeroFillWorkCode',
    'TimeClock_ShowRateOverrideOnBenefits',
    'TimeClock_AllowEditOfNotesOnLockedTimeCard',
    'TimeClock_AllowEditOfSchedulesOnLockedTimeCard',
    'TimeClock_AllowTimeOffRequestsFromPunchCalender',
    'Reporting_PointsReportsVisibleToEmployees',
    'Reporting_WhoWillPrintAcaReports',
    'TimeClock_CompanyAdminCanEditLockedTimeCards',
    'Payroll_ShowVoidCheck',
    'Reporting_MaskSocialSecurityNumber',
    'Payroll_EmployeeImport',
    'TimeClock_IPadPinNumberLength',
    'Reporting_PaycheckLogoPosition',
] as const;

