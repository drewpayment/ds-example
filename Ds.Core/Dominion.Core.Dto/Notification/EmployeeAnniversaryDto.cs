using Dominion.Core.Dto.Labor;
using System;

namespace Dominion.Core.Dto.Notification
{
    public class EmployeeAnniversaryDto
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime AnniversaryDate { get; set; }
        public string CostCenterDescription { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
        public int Years { get; set; }
    }
}
