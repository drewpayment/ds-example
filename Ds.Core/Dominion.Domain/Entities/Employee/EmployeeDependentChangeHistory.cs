using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeeDependentChangeHistory : Entity<EmployeeDependentChangeHistory>,
        IHasModifiedData,
        IHasChangeHistoryDataWithEnum,
       IEmployeeDependentEntity
    {
        public virtual int EmployeeDependentId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleInitial { get; set; }
        public virtual string SocialSecurityNumber { get; set; }
        public virtual string Relationship { get; set; }
        public virtual string Gender { get; set; }
        public virtual string Comments { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool TobaccoUser { get; set; }
        public virtual bool HasADisability { get; set; }
        public virtual bool IsAStudent { get; set; }
        public virtual int? EmployeeDependentsRelationshipId { get; set; }
        public virtual bool IsInactive { get; set; }
        public virtual DateTime? InactiveDate { get; set; }
        public virtual InsertStatus InsertStatus { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public int ChangeId { get; set; }
        public ChangeModeType ChangeMode { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
