using System;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates.Branch
{
    public class BranchDeductionLayout
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public float DeductionAmount { get; set; }
        public DateTime PayoutDate { get; set; }
        public string TransactionId { get; set; }
    }
}
