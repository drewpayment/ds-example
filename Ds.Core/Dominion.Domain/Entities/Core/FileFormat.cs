using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public class FileFormat : Entity<FileFormat>
    {
        public FileFormatEnum ScheduledReportFileFormatId { get; set; }
        public virtual string ScheduledReportFileFormat { get; set; }
    }
}
