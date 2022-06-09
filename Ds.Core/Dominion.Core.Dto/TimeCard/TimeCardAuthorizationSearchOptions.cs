using Dominion.Core.Dto.Core.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TimeCardAuthorizationSearchOptions : IHasPaginationPage
    {
        public int EmployeeId { get; set; }
        public ApprovalStatusType ApprovalStatusType { get; set; }
        public DaysFilterType DaysFilterType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PayPeriodDropdownSelectedValue { get; set; }
        public string PayPeriodDropdownSelectedItemText { get; set; }
        public TimeCardAuthorizationFilterDropdown Filter1Dropdown { get; set; }
        public TimeCardAuthorizationFilterDropdown Category1Dropdown { get; set; }
        public TimeCardAuthorizationFilterDropdown Filter2Dropdown { get; set; }
        public TimeCardAuthorizationFilterDropdown Category2Dropdown { get; set; }
        public int ClientId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
