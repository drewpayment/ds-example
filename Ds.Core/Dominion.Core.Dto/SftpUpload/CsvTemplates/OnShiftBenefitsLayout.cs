using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.SftpUpload.CsvTemplates
{
    public class OnShiftBenefitsLayout
    {
        public string facilityId { get; set; }
        public string employeeId { get; set; }
        public DateTime ptoStartDateTime { get; set; }
        public DateTime ptoEndDateTime { get; set; }
        public string reasonCode { get; set; }
        public string comment { get; set; }
    }
}
