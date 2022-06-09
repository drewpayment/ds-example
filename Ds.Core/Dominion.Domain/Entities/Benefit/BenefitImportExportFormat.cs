using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Benefit
{
    public class BenefitImportExportFormat
    {
        public int BenefitImportExportFormatId { get; set; }
        public bool IsImport                   { get; set; }
        public string Description              { get; set; }

        public virtual IList<BenefitImportExportType> BenefitImportExportTypes { get; set; }
    }
}
