using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public enum PayrollCheckSeqEnum : int
    {
        Employee                           = 1,
        Alphabetic                         = 2,
        Department                         = 3,
        Department_Alpha                   = 4,
        Shitft_Alpha                       = 5,
        Shift_Department                   = 6,
        Cost_Center_Alpha                  = 7,
        Pay_Type_Alpha_with_page_break     = 8,
        Cost_Center_Alpha_With_SubTotals   = 9,
        Department_With_SubTotals          = 10,
        Department_Alpha_With_SubTotals    = 11,
        Department_Shift_Alpha             = 12,
        Group_Department_Alpha             = 13,
        Pay_Type_Alpha                     = 15 // 13 to 15 in payrollcheckseq sql table ¯\_(ツ)_/¯
    }
}
