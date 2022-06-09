using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeEvent : Entity<EmployeeEvent>, IHasModifiedData
    {

        public int EmployeeEventID { get; set; }
        public int EmployeeID { get; set; }
        public int ClientSubTopicID { get; set; }
        public DateTime EventDate { get; set; }
        public string Event { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int ClientID { get; set; }
        public decimal Duration { get; set; }
        public bool IsEmployeeViewable { get; set; }
        public bool IsEmployeeEditable { get; set; }
        public virtual ClientSubTopic ClientSubTopic { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
