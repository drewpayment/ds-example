using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Forms
{
    public class FormSignatureDefinition : Entity<FormSignatureDefinition>, IHasModifiedData
    {
        public virtual int      SignatureDefinitionId { get; set; }
        public virtual int      FormDefinitionId      { get; set; }
        public virtual string   RoleIdentifier        { get; set; }
        public virtual string   RoleName              { get; set; }
        public virtual bool     IsRequired            { get; set; }
        public virtual int      ModifiedBy            { get; set; }
        public virtual DateTime Modified              { get; set; }

        public virtual FormDefinition FormDefinition { get; set; }
    }
}