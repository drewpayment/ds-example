using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    public class EmployeeGoal : Entity<EmployeeGoal>, IHasModifiedOptionalData
    {
        public int EmployeeId { get; set; }
        public int GoalId { get; set; }
        public int? OneTimeEarningSettingsId { get; set; }

        // RELATIONSHIPS

        public virtual Goal Goal { get; set; }
        public int? ModifiedBy { get; set; }

        public DateTime? Modified { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual OneTimeEarningSettings OneTimeEarning { get; set; }
    }
}
