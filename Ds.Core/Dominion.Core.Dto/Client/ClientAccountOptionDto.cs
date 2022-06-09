using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.Client
{
    public class ClientAccountOptionDto
    {
        public int ClientAccountOptionId              { get; set; }
        public int ClientId                           { get; set; }
        public ClientFullDto Client                   { get; set; }
        public AccountOption AccountOption            { get; set; }
        public AccountOptionInfoDto AccountOptionInfo { get; set; }
        public virtual string Value                   { get; set; }
    }
}
