using System.Collections.Generic;

namespace Dominion.Core.Dto.Forms
{
    public class FormStatusDto
    {
        public int?            FormId           { get; set; }
        public int?            EmployeeId       { get; set; }
        public int?            ClientId         { get; set; }
        public int             FormTypeId       { get; set; }
        public int             FormDefinitionId { get; set; }
        public string          FormName         { get; set; }
        public string          FormVersion      { get; set; }
        public bool            IsComplete       { get; set; }
        public bool            IsCurrentVersion { get; set; }
        public SystemFormType? SystemFormType    { get; set; }

        public FormDefinitionIdentifier DefinitionInfo { get; set; }

        public IEnumerable<FormSignatureStatusDto> Signatures { get; set; }
    }

    public class FormStatusWithData : FormStatusDto
    {
        public string JsonData   { get; set; }
        public string JsonForm   { get; set; }
    }
}
