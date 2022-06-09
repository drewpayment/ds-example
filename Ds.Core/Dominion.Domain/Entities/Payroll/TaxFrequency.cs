using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class TaxFrequency
    {
        public virtual int TaxFrequencyId { get; set; }
        public virtual string Frequency { get; set; }
    }
}
