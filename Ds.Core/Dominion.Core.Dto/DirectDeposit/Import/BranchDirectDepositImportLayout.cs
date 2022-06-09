using System;

namespace Dominion.Core.Dto.DirectDeposit.Import
{
    public class BranchDirectDepositImportLayout
    {
        public string Branch_UserID { get; set; }
        public string EID { get; set; }
        public string Employee_Name { get; set; }
        public DateTime Requested_Date { get; set; }
        public string Bank_Name { get; set; }
        public string Account { get; set; }
        public string Routing { get; set; }
    }
}
