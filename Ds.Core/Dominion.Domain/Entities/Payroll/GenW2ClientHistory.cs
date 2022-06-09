using System;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Payroll
{
    public class GenW2ClientHistory
    {
        public int GenW2ClientHistoryId { get; set; }
        public int ClientId { get; set; }
        public int W2Year { get; set; }
        public DateTime? EmployeesLastUpdatedOn { get; set; }
        public bool? RunLater { get; set; }

        public virtual Client Client { get; set; }
    }
}
