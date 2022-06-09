using Dominion.Core.Dto.Core;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Forms
{
    public class FormTypeIdentifier
    {
        public int             FormTypeId   { get; set; }
        public SystemFormType? SystemType   { get; set; }
        public LocalityType?   LocalityType { get; set; }
        public int?            LocalityId   { get; set; }
        public string          StateName    { get; set; }
        public List<int>       FieldIds     { get; set; }
    }
}
