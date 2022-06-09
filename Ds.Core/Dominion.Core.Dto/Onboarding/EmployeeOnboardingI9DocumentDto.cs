using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class EmployeeOnboardingI9DocumentDto
    {
        public int       EmployeeId       { get; set; }
        public int       I9DocumentId     { get; set; }
        public string    IssuingAuthority { get; set; }
        public string    DocumentNumber   { get; set; }
        public DateTime? ExpirationDate   { get; set; }
        public string    AdditionalInfo   { get; set; }
        public DateTime  CreatedDate      { get; set; }
        public DateTime  Modified         { get; set; }
        public int       ModifiedBy       { get; set; }
    }
}
