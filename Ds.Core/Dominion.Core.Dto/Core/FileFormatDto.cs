﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Core
{
    public class FileFormatDto
    {
        public FileFormatEnum ScheduledReportFileFormatId { get; set; }
        public string ScheduledReportFileFormat { get; set; }
    }
}
