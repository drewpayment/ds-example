namespace Dominion.Core.Dto.Billing
{
    /// <summary>
    /// Types of Billing Price Charts.  Corresponds to ID values in dbo.BillingPriceChart table.
    /// </summary>
    public enum BillingPriceChartType
    {
        Normal                = 0,
        OneTime               = 1,
        Discount10Percent     = 2,
        Discount20Percent     = 3,
        PriceChart1           = 4,
        W2Processing          = 5,
        AnnualInquirySheets   = 6,
        Discount15Percent     = 7,
        Discount05Percent     = 8,
        PriceChartWeekly      = 9,
        PriceChartBiWeekly    = 10,
        W2DominionPrint       = 11,
        LaborSourceWeekly     = 12,
        LabourSourceBiWeekly  = 13,
        YearEndAdjustment     = 14,
        Discount25Percent     = 15,
        Discount01Percent     = 16,
        Discount02Percent     = 17,
        Discount03Percent     = 18,
        Discount04Percent     = 19,
        Discount06Percent     = 21,
        Discount07Percent     = 22,
        Discount08Percent     = 23,
        Discount09Percent     = 24,
        Discount11Percent     = 25,
        Discount12Percent     = 26,
        Discount13Percent     = 27,
        Discount14Percent     = 28,
        Discount16Percent     = 29,
        Discount17Percent     = 30,
        Discount18Percent     = 31,
        Discount19Percent     = 32,
        Discount21Percent     = 33,
        Discount22Percent     = 34,
        Discount23Percent     = 35,
        Discount24Percent     = 36,
        Discount40Percent     = 37,
        Discount30Percent     = 38,
        Discount48Percent     = 39,
        Aca1095DominionPrints = 40,
        Aca1095ClientPrints   = 41
    }
}