using System;

namespace Dominion.Core.Dto.SftpConfiguration
{
    public enum SftpType : byte
    {
        DailyPayUserRosterExport          = 1,
        DailyPayGrossEarningsExport       = 2,
        DailyPayNetEarningsExport         = 3,
        DailyPayDirectDepositUpdateImport = 4,
        BranchUserRosterExport            = 5,
        BranchDirectDepositImport         = 6,
        BranchTimeAndAttendanceExport     = 7,
        BranchLocationFile                = 8,
        BenXpressImport = 9,
        BenXpressExport = 10,
        ICIMSImport = 11,
        ReliasEmployeeExport = 12,
        LiazonDeductionImport = 13,
        LiazonDemographicExport = 14,
        OnShiftDemographicExport = 15,
        OnShiftPunchExport = 16,
        OnShiftScheduleImport = 17,
        ChristianCareOnShiftDemographicExport = 18,
        ChristianCareOnShiftPunchExport = 19,
        MassMutual360Import = 20,
        OnShiftBenefitExport = 21,
        YardiExport = 22,

        /// <summary>
        /// Relates to <seealso cref="Dominion.Core.Dto.Payroll.ReportsToEmailType.ComPsychExportFile"/>
        /// </summary>
        ComPsychExport = 23,
    }
}
