using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Performance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PayrollRequestItemsDto
    {
        
        public int RequestType { get; set; }                    // type of payroll request
        public int ForeignKeyId { get; set; }                   // foreignkey to the type of payroll request (meritId or oneTimeId)
        public int ProposalId { get; set; }                     // proposal id
        public int ReviewId { get; set; }
        public int? ClientEarningId { get; set; }               // earning id for one time
        public BasedOn? BasedOn { get; set; }                      // dont know
        public int? EmployeeClientRateId { get; set; }          // rate id for merit increase
        public int EmployeeId { get; set; }    
        public string EmployeeNumber { get; set; }
        public PayFrequencyType PayFrequencyId { get; set; }    // dont know
        public IncreaseType IncreaseType { get; set; }          // percent or amount
        public double IncreaseAmount { get; set; }
        public int AnnualPayPeriodCount { get; set; }
        public double AnnualizedIncreaseAmount { get; set; }
        public double AnnualizedTotalAmount { get; set; }
        public IncreaseType? RecommendedIncreaseType { get; set; } // only apply to onetimeearning
        public decimal? RecommendedIncreaseAmount { get; set; } // only apply to onetimeearning
        public DateTime? EffectiveDate { get; set; }
        public ApprovalStatus ApprovalStatusId { get; set; }
        public double PayoutFrom { get; set; }
        public double PayoutTo { get; set; }
        public decimal Score { get; set; }
        public decimal? Percent { get; set; }
        public string DirectSupervisorName { get; set; }
        public string EmployeeJobTitle { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public string AwardDescription { get; set; }
        public User.UserDto DirectSupervisor { get; set; }
        public bool IsSelectedOnTableView { get; set; }
        public bool IsEnabledOnProposal { get; set; }
        public int CompletedGoals { get; set; }
        public int TotalGoals { get; set; }
        public bool ProcessedByPayroll { get; set; }
        public double BasePay { get; set; }
        /// <summary>
        /// Usually the Pay Type of the Employee but sometimes it is the Pay Type of a Merit Increase 
        /// </summary>
        public PayType? PayType { get; set; }
        public bool CanViewPayout { get; set; }
        public string EmployeeFirstName { get; set;  }
        public string EmployeeLastName { get; set; }
        public decimal? Target { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        /// <summary>
        /// Pay Type of the Employee.
        /// </summary>
        public PayType? EmployeePayType { get; set; }
    }
}
