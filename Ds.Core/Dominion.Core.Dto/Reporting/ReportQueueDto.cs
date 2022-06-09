using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Reporting
{
    public class ReportQueueDto
    {
        public     int                    ReportQueueId    { get; set; } // int
        public     int                    ClientId         { get; set; } // int
        public     int                    UserId           { get; set; } // int
        public     DateTime               RequestDate      { get; set; } // datetime
        public     DateTime?              StartDate        { get; set; } // datetime
        public     DateTime?              EndDate          { get; set; } // datetime
        public     StandardReportEnum     ReportId         { get; set; } // int
        public     string                 FileType         { get; set; } // varchar(15)
        public     string                 SessionId        { get; set; } // varchar(50)
        public     long                   Ticks            { get; set; } // bigint
        public     bool                   IsDeleted        { get; set; } // bit
        public     ReportQueueLogTypeEnum LogTypeId        { get; set; } // tinyint
        public     int                    PayrollId        { get; set; } // int
        public     string                 AzureBlobUri     { get; set; } // varchar(250)
        public     string                 AzureBlobName    { get; set; } // varchar(250)

        public ReportQueueDto ShallowCopy()
        {
            return (ReportQueueDto)this.MemberwiseClone();
        }
    }
}
