using System;

namespace Dominion.Core.Dto.Notification
{
    public class EmployeeNinetyDayAnniversaryDto
    {
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? HireDate { get; set; }
        public DateTime? RehireDate { get; set; }
        public DateTime? NinetyDayAnniversaryDate {
            get {

                DateTime NinetyDayAnniversaryDate = RehireDate.HasValue ?
                        new DateTime(RehireDate.Value.AddDays(90).Year, RehireDate.Value.AddDays(90).Month, RehireDate.Value.AddDays(90).Day) :
                        new DateTime(HireDate.Value.AddDays(90).Year, HireDate.Value.AddDays(90).Month, HireDate.Value.AddDays(90).Day);

                return NinetyDayAnniversaryDate;
            }
        }

        public string CostCenterDescription { get; set; }
        public string DepartmentName { get; set; }
    }
}
