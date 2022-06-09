namespace Dominion.Core.Dto.Client
{
    using System;
    using System.Collections.Generic;

    public class ClientSubTopicDto
    {
        public virtual int ClientSubTopicId { get; set; }
        public virtual int ClientTopicId { get; set; }
        public virtual string Description { get; set; }
        public virtual string TopicDescription { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }

        public virtual IEnumerable<ClientTopicDto> ClientTopic { get; set; }
    }
}
