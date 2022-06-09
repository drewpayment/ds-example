using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Forms
{
    public class FormDefinitionFields : Entity<FormDefinitionFields>
    {
        public virtual int FormDefinitionId { get; set; }
        public virtual int FieldId { get; set; }

        public virtual FormDefinition FormDefinition { get; set; }

        public virtual FormFields FormFields { get; set; }
    }
}
