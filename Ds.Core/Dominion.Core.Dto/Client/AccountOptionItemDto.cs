using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class AccountOptionItemDto
    {
        public int           AccountOptionItemId { get; set; }
        public AccountOption AccountOption       { get; set; }
        public string        Description         { get; set; }
        public bool          IsDefault           { get; set; }
        public byte?         Value               { get; set; }
    }
}