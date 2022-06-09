namespace Dominion.Core.Dto.Misc
{
    /// <summary>
    /// Account features available to Clients
    /// </summary>
    /// <remarks>
    /// NOTE: The integer values must match the FeatureOptions table id values. Based on dbo.FeatureOptions
    /// </remarks>
    public enum AccountFeatureEnum : int
    {
        CompanyAdmin_Reporting = 1,
        GeneralLedger = 2,
        Interface_401k = 3,
        CompanyAdmin_EmployeeSelfService = 4,
        TimeClock = 5,
        BlockPayrollAccess = 6,
        EmployeeChangeRequests = 7,
        ApplicantTracking = 8,
        EeocReporting = 9,
        ElectronicW2Consent = 10,
        EmployeePointAndIncidents_Manual = 11,
        CrossClientCodeReporting = 12,
        ApplicantTracking_Hiring_Workflow = 13,
        //WorkOrderApproval = 14,
        JobCosting = 15,
        EmployeePointsAndIncidents_Auto = 16,
        MobileApplication = 17,

        /// <summary>
        /// Indicates if Quick Scheduling (previously Group Scheduling) is enabled for the current client.
        /// </summary>
        QuickScheduling = 18,
        EmployeeBenefits_ImportExport = 19,
        New_ESS = 20,
        Benefit_Portal = 21,
        Expensify = 22,
        ArcadiaFsa = 23,

        /// <summary>
        /// Indicates if new Group Scheduler feature is enabled for the current client.
        /// </summary>
        GroupScheduler = 24,
        AcaReporting = 25,
        CarrierConnections = 26,
        OnBoarding = 27,
        EeocFiling = 28,
        SplitTaxesByCostCenter = 29,
        Budgeting = 30,
        Expectancy = 31,
        PerformanceReviews = 32,
        GoalTracking = 33,
        PunchImport = 34,
        ICIMSEmployeeImport = 35,

        /// <summary>
        /// Indicates if Nobscot WebExit integration feature is enabled for the current client.
        /// </summary>
        RequestNobscotExitInterviews = 37,
        DailyPayIntegration = 38,
        BranchIntegration = 39,
        Geofencing = 40,
        EmployeeNavigator = 41,
        YardiExport = 42,
    }
}
