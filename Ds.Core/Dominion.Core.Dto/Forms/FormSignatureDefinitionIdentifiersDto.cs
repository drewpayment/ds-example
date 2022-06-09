namespace Dominion.Core.Dto.Forms
{
    public class FormSignatureDefinitionIdentifiersDto
    {
        public int    SignatureDefinitionId { get; set; }
        public int FormDefinitionId { get; set; }
        public string RoleIdentifier        { get; set; }
        public bool   IsRequired            { get; set; }
    }
}