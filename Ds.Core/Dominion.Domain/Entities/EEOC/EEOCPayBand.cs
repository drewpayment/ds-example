using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.EEOC
{
    public class EEOCPayBand : Entity<EEOCPayBand>
    {
        public virtual int      EeocPayBandId              { get; set; }
        public virtual int      PayBand                    { get; set; }
        public virtual decimal  LowerThreshold             { get; set; }
        public virtual decimal  UpperThreshold             { get; set; }
        public virtual int      W2Year                     { get; set; }

    }
}
