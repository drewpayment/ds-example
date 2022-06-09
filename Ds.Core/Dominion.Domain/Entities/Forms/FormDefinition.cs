using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Forms
{
    public class FormDefinition : Entity<FormDefinition>, IHasModifiedData
    {
        public virtual int       FormDefinitionId     { get; set; }
        public virtual int       FormTypeId           { get; set; }
        public virtual string    Name                 { get; set; }
        public virtual string    Version              { get; set; }
        public virtual int       ModifiedBy           { get; set; }
        public virtual DateTime  Modified             { get; set; }
        public virtual DateTime? EffectiveFromDate    { get; set; }
        public virtual DateTime? EffectiveToDate      { get; set; }
        public virtual int?      OverrideDefinitionId { get; set; }

        public virtual FormType FormType { get; set; }

        public virtual FormDefinition OverrideDefinition { get; set; }

        public virtual ICollection<FormSignatureDefinition> SignatureDefinitions { get; set; }
        public virtual ICollection<FormDefinitionFields> FormDefinitionFields { get; set; }
    }
}