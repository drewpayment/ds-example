using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientPayrollRunDto
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string DataEntry { get; set; }
        public string Frequency { get; set; }
        public string ClientCode { get; set; }
        public string ProcessDay { get; set; }
        public int ProcessDayId { get; set; }
        public string Status { get; set; }
        public DateTime LastCheckDateRun { get; set; }
        public DateTime LastPayrollAccepted { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string PostalCode { get; set; }
        public ClientContactDto Contact { get; set; }
        public bool RunningThisWeek { get; set; }
        public bool ValidPayroll { get; set; }
    }
}
