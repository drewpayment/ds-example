namespace Dominion.Core.Dto.Payroll
{
    /// <summary>
    /// Types of employee deduction amounts.
    /// </summary>
    public enum EmployeeDeductionAmountType
    {
        PercentOfNet                         = -1,
        PercentOfGross                       = -2,
        Flat                                 = -3,
        PercentOfPartialGross                = -4,
        PercentOfPartialGrossFlex            = -5,
        GrossMinusFlex                       = -6,
        GrossMinusTaxesMinusFlex             = -7,
        FlatTimesAllHours                    = -8,
        FlatTimesAllMinusOtherHours          = -9,
        PercentOfRegularPay                  = -10,
        Garnishment_GrossMinusTaxes_L        = -11,
        Garnishment_GrossMinusTaxes_M        = -12,
        Garnishment_GrossMinusTaxesMinusFlex = -13,
        Garnishment_IRS                      = -14,
        FlatAsNetPayOnAllnet                 = -15,
        WhatsLeftFlat                        = -16,
        SSOptOut_MinusLTD                    = -17,
        EIC_Married                          = -18,
        EIC_Single                           = -19,
        FicaOverride                         = -20,
        MIPFixed                             = -21,
        MIPGraded                            = -22,
        MIPPlus                              = -23,
        MIPBasic                             = -26,
        MIPHyrbid                            = -27,
        PlusTwoHalfTimesRateA                = -24,
        VariableIns                          = -25,
        FlatTimesAllHoursTimeEarningPercent  = -28,
        GrandTransformers_UnionDues          = -29,
        GrossMinusTaxesMinusMedical          = -30, //Used by one client for a garnishment CFU1
        DCU1CustomGross                      = -31,
        FriendOfTheCourtPercentofNet         = -32,
        GrossMinusTaxes                      = -33,
        ChallengeMachinery_UnionDues         = -34,
        ReimburseTaxAmount                   = -35,
        FlatTimesPartialGrossHours           = -36,
        PercentGrossMinusExcessInsurance     = -37,
        PlusDoubleRateA                      = -38


    }
}