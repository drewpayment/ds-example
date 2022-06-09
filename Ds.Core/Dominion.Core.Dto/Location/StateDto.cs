namespace Dominion.Core.Dto.Location
{
    public class StateDto
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public string Abbreviation { get; set; }

        public string PostalNumericCode { get; set; }

        public int? FormTypeId { get; set; }
        public bool? NoDefinitionRequired { get; set; }
        
       // public  Counties {get; set;}
    }
}