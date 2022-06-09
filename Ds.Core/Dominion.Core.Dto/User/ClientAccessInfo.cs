using System.Collections.Generic;

namespace Dominion.Core.Dto.User
{
    public class ClientAccessInfo
    {
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public IEnumerable<ClaimTypeDto> Claims { get; set; }
    }
}