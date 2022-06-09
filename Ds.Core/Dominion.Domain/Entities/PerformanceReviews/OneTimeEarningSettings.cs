using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class OneTimeEarningSettings : Entity<OneTimeEarningSettings>, IHasModifiedData
    {
        public int OneTimeEarningSettingsId { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public IncreaseType IncreaseType { get; set; }
        public decimal IncreaseAmount { get; set; }
        public BasedOn BasedOn { get; set; }
        public Measurement Measurement { get; set; }
        public bool DisplayInESS { get; set; }
        /// <summary>
        /// When true a one time earning cannot be created for the <see cref="Employee"/>
        /// </summary>
        public bool IsArchived { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public virtual Employee.Employee Employee { get; set; }
        public virtual ICollection<EmployeeGoal> EmployeeGoals { get; set; }
    }
}
