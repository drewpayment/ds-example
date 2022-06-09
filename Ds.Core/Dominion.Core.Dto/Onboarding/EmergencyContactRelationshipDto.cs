using System;


namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmergencyContactRelationshipDto
    {
        public int RelationshipId { get; set; }
        public string Description { get; set; } 
    }
}
