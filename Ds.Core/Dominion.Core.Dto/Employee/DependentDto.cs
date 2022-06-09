using System;

using Dominion.Core.Dto.Contact;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeeDependentDto2 : CommonContactPersonalDto2
    {
        public string Relationship { get; set; }
        public bool   IsAStudent { get; set; }
        public bool   HasADisability { get; set; }
        public bool   TobaccoUser { get; set; }
        public int    EmployeeDependentId { get; set; }

        public EmployeeDependentPcpDto PrimaryCarePhysician { get; set; }
  
    }
}