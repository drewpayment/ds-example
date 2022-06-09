using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientTaxInfoDto
    {
        public virtual int ClientTaxInfoId { get; set; }
        public virtual int ClientTaxId { get; set; }
        public virtual DateTime EffectiveDate { get; set; }
        public virtual double Rate { get; set; }
        public virtual double? Limit { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string Modifiedby { get; set; }

        public ClientTaxDto Tax { get; set; }
    }
}
