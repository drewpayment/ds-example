namespace Dominion.Core.Dto.Forms
{
    public class FormSignatureStatusDto : SignatureDto
    {
        public int       SignatureDefinitionId { get; set; }
        public string    RoleIdentifier        { get; set; }
        public string    RoleName              { get; set; }
        public bool      IsSigned              { get; set; }
    }
}
