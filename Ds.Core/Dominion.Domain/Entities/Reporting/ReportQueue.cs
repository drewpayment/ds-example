using Dominion.Core.Dto.Reporting;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Reporting
{
    public class ReportQueue : Entity<ReportQueue>
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

        // REVERSE NAVIGATION
        //public virtual Client Client { get; set; }
        //public virtual User.User User { get; set; }
        //public virtual Payroll.Payroll Payroll { get; set; }
        //public virtual StandardReport StandardReport { get; set; }
    }
}
