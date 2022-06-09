using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;

namespace Dominion.Domain.Entities.Forms
{
    public class FormSignature : Entity<FormSignature>
    {
        public virtual int FormId                { get; set; }
        public virtual int SignatureId           { get; set; }
        public virtual int SignatureDefinitionId { get; set; }

        public virtual Form                    Form                { get; set; }
        public virtual Signature               Signature           { get; set; }
        public virtual FormSignatureDefinition SignatureDefinition { get; set; }
    }
}