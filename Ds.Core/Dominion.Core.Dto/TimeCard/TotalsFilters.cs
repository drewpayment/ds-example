using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class TotalsFilters
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EmployeeFilter Filter1 { get; set; }
        public EmployeeFilter Filter2 { get; set; }
        public ApprovalStatusType ApprovalStatus { get; set; } = ApprovalStatusType.All;
        public ApprovalOptionType ApprovalOption { get; set; }
        public DaysFilterType DaysFilter { get; set; }

        public int ClientID { get; set; }

        public int UserID { get; set; }

        public int UserTypeID { get; set; }

        public int SupervisorEmployeeID { get; set; }

        public bool ShowCostCenterTooltip { get; set; }

        public bool ShowHoursInHundredths { get; set; }

        public TypeOfHoursType TypeOfHours { get; set; }
        public int EmployeeID { get; set; }
        public int UserEmployeeID { get; set; }
        public bool HasNotes { get; set; }

        public bool AllowAddPunches { get; set; } = true;

        public bool AllowEditPunches { get; set; } = true;

        public bool AllowAddBenefits { get; set; } = true;

        public bool AllowEditBenefits { get; set; } = true;

        public bool ShowDailyTotals { get; set; }

        public bool ShowWeeklyTotals { get; set; }

        public bool ShowGrandTotals { get; set; }

        public bool BlockSupervisorsFromAuthorizingOwnTimecards { get; set; }

        public bool SplitTotalsByShift { get; set; }

        /// <summary>
        ///         ''' The current date and time. 
        ///         ''' Used whenever the current time needs to be compared to another time.
        ///         ''' Defaults to DateTime.Now.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public DateTime CurrentTime { get; set; } = DateTime.Now;

        /// <summary>
        ///         ''' The list of employees that the supervisor has been given explicit access to.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public virtual IEnumerable<Employee> TrueEmployees { get; set; } = Enumerable.Empty<Employee>();

        /// <summary>
        ///         ''' The number of employees that should be shown per page.
        ///         ''' </summary>
        ///         ''' <returns></returns>
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int EmployeeCount { get; set; }
        public int TotalPages { get; set; }
    }
}
