using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Dashboard
{
    public class DashboardSessionDto
    {
        public int UserId { get; set; }
        public int DashboardId { get; set; }
        public string FilterData { get; set; }
        public DashboardConfigDto DashboardConfig {get;set;}
    }

    public class DashboardSearchFilters
    {
        public string DateRangeType { get; set; }
        public DashboardFilterDateRange DateRange { get; set; }
        public int? CostCenter { get; set; }
        public int? Division { get; set; }
        public int? Department { get; set; }
        public int? JobTitle { get; set; }
        public int? Group { get; set; }
        public int? Shift { get; set; }
        public int? EmployeeStatus { get; set; }
        public int? PayType { get; set; }
        public int? TimePolicy { get; set; }
        public int? Supervisor { get; set; }
    }

    public class DashboardFilterDateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
