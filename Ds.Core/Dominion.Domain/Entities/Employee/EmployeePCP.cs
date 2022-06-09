using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    public class EmployeePcp : Entity<EmployeePcp>,IHasModifiedData
    {
   
        public virtual int EmployeeId { get; set; }
      
        public virtual string FirstName { get; set; }

        public virtual string LastName  { get; set; }
        public virtual string Address  { get; set; }
        public virtual string Address2  { get; set; }
        public virtual string City  { get; set; }
        public virtual int StateId   { get; set; }
        public virtual string ZipCode  { get; set; }
        public virtual string NpiNumber  { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }

        public virtual Employee.Employee Employee { get; set; }
        public virtual State State { get; set; }

        public EmployeePcp()
        {
        }

        #region Filters



        #endregion
    }
}