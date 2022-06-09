using System;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeEventDto
    {
        public int EmployeeEventId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientTopicId { get; set; }
        public string ClientTopicDescription { get; set; }
        public int ClientSubTopicId { get; set; }
        public string ClientSubTopicDescription { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Event { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public int ClientId { get; set; }
        public decimal Duration { get; set; }
        public bool IsEmployeeViewable { get; set; }
        public bool IsEmployeeEditable { get; set; }
    }
}
