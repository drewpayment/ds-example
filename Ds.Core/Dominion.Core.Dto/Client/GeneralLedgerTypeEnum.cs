using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public enum GeneralLedgerTypeEnum
    {
        // CASH
        CASH_EmployeeChecks = 1,
        CASH_EmployeeDD = 2,
        CASH_VendorChecks = 3,
        CASH_VendorDD = 4,
        CASH_AllTaxes = 5,
        CASH_AllChecks = 33,
        CASH_DominionChecks = 36,
        CASH_AdjustmentChecks = 38,
        CASH_F941 = 40,
        CASH_TMCatchup = 43,
        LIABILITY_FederalTax = 6,
        LIABILITY_SocialSecurity = 7,
        LIABILITY_Medicare = 8,
        LIABILITY_EmployerSocSec = 9,
        LIABILITY_EmployerMedicare = 10,
        LIABILITY_StateTax = 11,
        LIABILITY_LocalTax = 12,
        LIABILITY_SUTATax = 13,
        LIABILITY_FUTATax = 14,
        LIABILITY_Deductions = 15,
        LIABILITY_GLAccrualLiability = 35,
        LIABILITY_WorkersCompCredit = 41,
        EXPENSE_Earnings = 16,
        EXPENSE_GrossPay = 17,
        EXPENSE_EmployerSocSec = 18,
        EXPENSE_EmployerMedicare = 19,
        EXPENSE_FUTA = 20,
        EXPENSE_SUTA = 21,
        EXPENSE_Match = 34,
        EXPENSE_Memo = 37,
        EXPENSE_PremiumPay = 39,
        EXPENSE_WorkersCompDebit = 42,
        PAYMENT_FederalTax = 22,
        PAYMENT_SocialSecurity = 23,
        PAYMENT_Medicare = 24,
        PAYMENT_EmployerSocSec = 25,
        PAYMENT_EmployerMedicare = 26,
        PAYMENT_StateTax = 27,
        PAYMENT_LocalTax = 28,
        PAYMENT_SUTA = 29,
        PAYMENT_FUTA = 30,
        PAYMENT_ClientVendor = 31,
        PAYMENT_DominionVendor = 32,
    }
}
