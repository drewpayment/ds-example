using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    /// <summary>
    /// This dto represents a call to 'spGetRequestTimeOffBalances' that is called to return on 2 of the 3 tables it can return.
    /// </summary>
    public class RequestTimeOffDetailsSprocDto
    {
        //added for correlation
        public int          RequestTimeOffId           { get; set; }

        //1st dataset
        public int?          RequestBalanceStatus       { get; set; }
        public decimal?      BalanceBeforeRequest       { get; set; }
        public decimal       RequestBalance             { get; set; }
        public decimal?      GenBalance                 { get; set; }
        public decimal?      ApprovedUnits              { get; set; }
        public decimal?      PendingUnits               { get; set; }
        public DateTime?     BeforeCurrentRelevantDate  { get; set; }
        public DateTime?     LastPayrollDate            { get; set; }
        public DateTime?     PayrollBegin               { get; set; }
        public DateTime?     PayrollEnd                 { get; set; }

        //2nd dataset
        public TimeOffNotesDto Notes { get; set; }

    }
}

//public string ApproverNotes     { get; set; }
//public string RequesterNotes    { get; set; }
