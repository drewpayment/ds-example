using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;


namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeDriversLicense :
        Entity<EmployeeDriversLicense>, 
        IHasModifiedData
    {
        public virtual int EmployeeId { get; set; }
        public virtual string DriversLicenseNumber { get; set;}
        public virtual int? IssuingStateId { get; set;}
        public virtual DateTime? ExpirationDate { get; set; }
        public virtual int ClientId { get; set; } 
        public virtual State IssuingState { get; set;}
        public virtual Employee Employee { get; set; } 
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get;  set; }

            public EmployeeDriversLicense()
        {
        }
  
    }
}