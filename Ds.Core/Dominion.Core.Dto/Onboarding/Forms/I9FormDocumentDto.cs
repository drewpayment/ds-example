using System;

namespace Dominion.Core.Dto.Onboarding.Forms
{
    public class I9FormDocumentDto
    {
        public string    DocumentTitle    { get; set; }
        public string    IssuingAuthority { get; set; }
        public string    DocumentNumber   { get; set; }
        public DateTime? ExpirationDate   { get; set; }
        public string AdditionalInfo { get; set; }
    }
}