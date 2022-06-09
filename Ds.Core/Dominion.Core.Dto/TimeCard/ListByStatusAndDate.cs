using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ListByStatusAndDate
    {
        public class Row
        {
            public string Description { get; set; }
            public DateTime RequestDate { get; set; }
            public int RequestTimeOffID { get; set; }
            public double Hours { get; set; }
            public int EmployeeID { get; set; }

            public string EmployeeName { get; set; }
            public int Status { get; set; }
        }

        public IEnumerable<Row> Rows { get; set; }
    }
}
