using System;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Accruals
{
    public class EmployeeAccrualSummaryDto : IHasServiceReferencePointDateInfo
    {
        public int                 EmployeeId                { get; set; }
        public int                 ClientId                  { get; set; }
        public DateTime?           BirthDate                 { get; set; }
        public int?                Age                       { get; set; }
        public DateTime?           HireDate                  { get; set; }
        public DateTime?           EligibilityDate           { get; set; }
        public DateTime?           AnniversaryDate           { get; set; }
        public DateTime?           ReHireDate                { get; set; }
        public EmployeeStatusType? EmployeeStatus            { get; set; }
        public string              EmployeeStatusDescription { get; set; }
        public PayType?            PayType                   { get; set; }
        public string              PayTypeDescription        { get; set; }
        public int?                ClientDepartmentId        { get; set; }
        public string              ClientDepartmentCode      { get; set; }
    }
}