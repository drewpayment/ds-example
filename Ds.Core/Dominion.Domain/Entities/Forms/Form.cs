using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Forms
{
    public class Form : Entity<Form>, IHasModifiedData
    {
        public virtual int      FormId           { get; set; }
        public virtual int      FormDefinitionId { get; set; }
        public virtual string   JsonData         { get; set; }
        public virtual string   JsonForm         { get; set; }
        public virtual bool     IsComplete       { get; set; }
        public virtual int      ModifiedBy       { get; set; }
        public virtual DateTime Modified         { get; set; }

        public virtual FormDefinition         FormDefinition     { get; set; }
        public virtual Resource               Resource           { get; set; }
        public virtual EmployeeOnboardingForm OnboardingFormInfo { get; set; }

        public virtual ICollection<FormSignature> FormSignatures { get; set; }
    }
}
