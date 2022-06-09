using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    public class EmployeeDependentPcp : Entity<EmployeeDependentPcp>,IHasModifiedData
    {
       
        public virtual int EmployeeDependentId { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
        public virtual string Address { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string City { get; set; }
        public virtual int StateId { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string NpiNumber { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }

        public virtual EmployeeDependent Dependent { get; set; }
        public virtual State State { get; set; }

        public EmployeeDependentPcp()
        {
        }

        #region Filters



        #endregion
    }
}