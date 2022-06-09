using System;

using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.Extensions;

namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public class ContactNameDto2 : IContactNameDto, IHasFirstMiddleInitialLast
    {
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }

        public virtual void Normalize()
        {
            this.NormalizeContactName();
        }
    }
}