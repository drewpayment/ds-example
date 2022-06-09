using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Onboarding
{
    public class EmergencyContactRelationship : Entity<EmergencyContactRelationship>
    {
        public virtual int RelationshipId { get; set; }
        public virtual string Description { get; set; } 
    }
}
