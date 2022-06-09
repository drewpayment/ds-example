using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class EmployeeDependentRelationships
    {
        public virtual int    EmployeeDependentsRelationshipId { get; set; }
        public virtual string Description                      { get; set; }
        public virtual bool   IsSpouse                         { get; set; }
        public virtual bool   IsChild                          { get; set; }
    }
}
