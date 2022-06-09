using System.Collections.Generic;
using Dominion.Core.Dto.Core;

namespace Dominion.Core.Dto.Forms
{
    public class FormDefinitionIdentifier : FormTypeIdentifier
    {
        public int             FormDefinitionId { get; set; }
        public string          FormName         { get; set; }
        public string          Version          { get; set; }

        public IEnumerable<FormSignatureDefinitionIdentifiersDto> Signatures { get; set; }
         public LocalityType?   OrigLocalityType { get; set; }
        public int?             OrigLocalityId   { get; set; }            
    }
}
