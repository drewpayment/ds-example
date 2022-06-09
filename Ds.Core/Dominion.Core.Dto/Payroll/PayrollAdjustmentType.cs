
namespace Dominion.Core.Dto.Payroll
{
    public enum PayrollAdjustmentType
    {
        DesiredNetPay             = 1,
        FederalTaxes              = 2,
        SocialSecurityTaxes       = 3,
        Medicare                  = 4,
        Taxes                     = 5,
        Deductions                = 6,
        DirectDeposits            = 7,
        Bonds                     = 8,
        Earnings                  = 9,
        Accruals                  = 10,
        SUTA                      = 11,
        TaxableWages              = 12,
        SocialSecurityWages       = 13,
        MedicareWages             = 14,
        FUTAWages                 = 15,
        EmployerSocialSecurityTax = 16,
        EmployerMedicareTax       = 17,
        EmployerFUTATax           = 18,
        ExemptWages               = 19,
        FlexDeductions            = 20,
        LimitWages                = 21,
        ExcessWages               = 22,
        SUTAAmount                = 23,
        SUTAFUTAWages             = 24,
        SUTAFUTATax               = 25,
        GrossPay                  = 26,
        PartialGrossPay           = 27,
        Tips                      = 28,
        PartialTips               = 29,
        CheckAmount               = 30,
        NetPay                    = 31,
        SuccessorSocSecWages      = 32,
        SuccessorMedicareWages    = 33,
        SuccessorFUTAWages        = 34,
        SuccessorSUTAWages        = 35,
        SuccessorDisabilityWages  = 36
    }
}
