using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    class EmployeeOnboardingMenuClientDto
    {
        public virtual int      ClientId   { get; set; }
        public virtual byte     SeqNo      { get; set; }
        public virtual byte     MenuId     { get; set; }
        public virtual int      ModifiedBy { get; set; }
        public virtual DateTime Modified   { get; set; } 
    }
}
