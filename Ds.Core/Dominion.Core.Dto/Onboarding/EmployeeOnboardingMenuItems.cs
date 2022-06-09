using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    class EmployeeOnboardingMenuItems
    {
        public virtual byte     MenuId        { get; set; }
        public virtual byte     MenuItemId    { get; set; }
        public virtual byte?    MenuSubItemId { get; set; }
        public virtual string   MenuTitle     { get; set; }
        public virtual string   LinkToState   { get; set; }
        public virtual int      ModifiedBy    { get; set; }
        public virtual DateTime Modified      { get; set; } 
    }
}
