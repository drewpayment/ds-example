using System;


namespace Dominion.Core.Dto.Onboarding
{
    public partial class EmployeeDependentRelationshipsDto
    {
        public int    EmployeeDependentsRelationshipId { get; set; }
        public string Description                      { get; set; }
        public bool   IsSpouse                         { get; set; }
        public bool   IsChild                          { get; set; }
    }
}
