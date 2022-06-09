using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    public class ClientTopicDto
    {
        public int ClientTopicId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }

        public IEnumerable<ClientSubTopicDto> ClientSubTopics { get; set; }
    }
}
