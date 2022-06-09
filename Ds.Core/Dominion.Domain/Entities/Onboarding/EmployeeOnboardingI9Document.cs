using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{

        public partial class EmployeeOnboardingI9Document : Entity<EmployeeOnboardingI9Document>, IHasModifiedData
    {
        public virtual int EmployeeId { get; set; }
        public virtual int I9DocumentId { get; set; }
        public virtual string IssuingAuthority { get; set; }
        public virtual string DocumentNumber { get; set; }
        public virtual DateTime? ExpirationDate { get; set; }
        public virtual string AdditionalInfo { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual I9Document I9Document { get; set; }
    }
}
