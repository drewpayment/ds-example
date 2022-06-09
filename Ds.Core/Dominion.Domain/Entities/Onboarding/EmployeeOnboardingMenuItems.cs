using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public class EmployeeOnboardingMenuItems : Entity<EmployeeOnboardingMenuItems>, IHasModifiedData
    {
        public virtual byte     MenuId        { get; set; }
        public virtual byte     MenuItemId    { get; set; }
        public virtual byte?    MenuSubItemId { get; set; }
        public virtual string   MenuTitle     { get; set; }
        public virtual string   LinkToState   { get; set; }
        public virtual string   Route         { get; set; }
        public virtual int      ModifiedBy    { get; set; }
        public virtual DateTime Modified      { get; set; } 
    }
}
