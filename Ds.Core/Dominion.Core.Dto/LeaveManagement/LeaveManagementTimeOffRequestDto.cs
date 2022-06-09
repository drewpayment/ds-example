using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class LeaveManagementTimeOffRequestDto
    {
        public int RequestTimeOffId { get; set; }
        public int ClientAccrualId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime RequestFrom { get; set; }
        public DateTime RequestUntil { get; set; }
        public int ModifiedBy { get; set; }
        public decimal AmountInOneDay { get; set; }
        public string RequesterNotes { get; set; }
    }
}
