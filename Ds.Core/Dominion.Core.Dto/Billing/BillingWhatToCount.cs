using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Billing
{
    /**
     * Update front end enum as well (billing-what-to-count.enum.ts)
     */
    public enum BillingWhatToCount
    {
        Checks = 1,
        PayTransactions = 2,
        Deposits = 3,
        FileChanges = 4,
        InquirySheets = 5,
        ActiveEmployees = 6,
        NewHires = 7,
        PayGridRecords = 8,
        TaxFilings = 9,
        W2Forms = 10,
        ChecksAdjDeposits = 11,
        Taxchecks = 12,
        FreePayrolls = 13, //9/16/2008 ronk
        ChecksOnly = 14, //1/9/09 Ronk
        AdvicesOnly = 15, //1/9/09 Ronk
        TimeClockEmployees = 16, //2/2/09 Ronk,
        FreePayrollTier2 = 17, //4/9/09 Ronk
        ChecksAdvicesLessVendorAdvices = 18, //8/20/2009
        LeaveMgmtEmployees = 19, //2/1/10
        ApplicantPostings = 20, //10/25/2011 Ronk
        BenefitEmployees = 21, //10/13/2014 Ronk
        ACAForms = 22, // 10/21/2015
        OnboardingEmployees = 23, //9/9/2016 Ronk
        PerformanceEmployees = 24,
        GeofenceEmployees = 25
    }

    static class BillingWhatToCountExtensions
    {
        public static string ToString(this BillingWhatToCount whatToCount)
        {
            switch (whatToCount)
            {
                case BillingWhatToCount.Checks:
                    return "Checks/Advices";
                case BillingWhatToCount.PayTransactions:
                    return "Pay Transactions";
                case BillingWhatToCount.Deposits:
                    return "Deposits";
                case BillingWhatToCount.FileChanges:
                    return "File Changes";
                case BillingWhatToCount.InquirySheets:
                    return "Inquiry Sheets";
                case BillingWhatToCount.ActiveEmployees:
                    return "Active Employees";
                case BillingWhatToCount.NewHires:
                    return "New Hires";
                case BillingWhatToCount.PayGridRecords:
                    return "Pay Grid Records";
                case BillingWhatToCount.TaxFilings:
                    return "Tax Filings";
                case BillingWhatToCount.W2Forms:
                    return "W2 Forms";
                case BillingWhatToCount.ChecksAdjDeposits:
                    return "Checks/Adj/Deposits";
                case BillingWhatToCount.Taxchecks:
                    return "Taxchecks";
                case BillingWhatToCount.FreePayrolls:
                    return "Free Payrolls";
                case BillingWhatToCount.ChecksOnly:
                    return "Checks Only";
                case BillingWhatToCount.AdvicesOnly:
                    return "Advices Only";
                case BillingWhatToCount.TimeClockEmployees:
                    return "Time Clock Employees";
                case BillingWhatToCount.FreePayrollTier2:
                    return "Free Payrolls(Tier 2)";
                case BillingWhatToCount.ChecksAdvicesLessVendorAdvices:
                    return "Checks/Adv/LessVendorAdv";
                case BillingWhatToCount.LeaveMgmtEmployees:
                    return "Leave Mgmt Employees";
                case BillingWhatToCount.ApplicantPostings:
                    return "Applicant Postings";
                case BillingWhatToCount.BenefitEmployees:
                    return "Benefit Employees";
                case BillingWhatToCount.ACAForms:
                    return "ACA Forms";
                case BillingWhatToCount.OnboardingEmployees:
                    return "Onboarding Employees";
                case BillingWhatToCount.PerformanceEmployees:
                    return "Performance Employees";
                case BillingWhatToCount.GeofenceEmployees:
                    return "Geofence Employees";
                default:
                    throw new ItemToCountHasNoStringRepresentation(whatToCount);
            }
        }
    }

    public class ItemToCountHasNoStringRepresentation : Exception
    {
        private static string message = "That billing item type has no string representation defined in the 'BillingWhatToCountExtensions' class!";
        public ItemToCountHasNoStringRepresentation(BillingWhatToCount itemType)
            :base(message + "  The type was: " + itemType)
        { }

        public ItemToCountHasNoStringRepresentation()
            :base (message)
        { }
    }
}
