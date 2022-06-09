using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Core
{
    public class TermsAndConditionsVersion : Entity<TermsAndConditionsVersion>
    {
        public int TermsAndConditionsVersionID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? LastEffectiveDate { get; set; }
        public string FileName { get; set; }
        public string FileLocation { get; set; }
    }
}
