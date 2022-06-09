using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public class SavedReportCustomFieldDto
    {
        public int SavedReportCustomFieldId { get; set; }
        public string Description { get; set; }
        public int ClientId { get; set; }
        public string CustomOperator { get; set; }
        public double? CustomValue { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

    }
}
