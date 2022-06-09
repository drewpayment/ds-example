using System;

namespace Dominion.Core.Dto.Forms
{
    public class SignatureDto
    {
        public int?      SignatureId     { get; set; }
        public string    SignatureName   { get; set; }
        public DateTime? SignatureDate   { get; set; }
        public string    SigneeFirstName { get; set; }
        public string    SigneeLastName  { get; set; }
        public string    SigneeMiddle    { get; set; }
        public string    SigneeInitials  { get; set; }
        public string    SigneeTitle     { get; set; }
        public int       ModifiedBy      { get; set; }
    }
}
