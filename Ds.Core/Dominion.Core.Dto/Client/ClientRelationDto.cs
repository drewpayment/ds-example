using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public class ClientRelationDto
    {
        public int ClientRelationId { get; set; }
        public string  Name { get; set; }
        public IEnumerable<ClientDto> Clients { get; set; }

    }
}
