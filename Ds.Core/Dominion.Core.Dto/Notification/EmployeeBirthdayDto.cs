using System;

namespace Dominion.Core.Dto.Notification
{
    public class EmployeeBirthdayDto
    {
        public int      ClientId    { get; set; }
        public int      EmployeeId  { get; set; }
        public string   FirstName   { get; set; }
        public string   LastName    { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CostCenterDescription { get; set; }
        public string DepartmentName { get; set; }
        public bool IsActive { get; set; }
    }
}
