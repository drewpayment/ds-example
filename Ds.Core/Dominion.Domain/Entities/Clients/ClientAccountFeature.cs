using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using System;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Client Feature Entity
    /// </summary>
    public class ClientAccountFeature : Entity<ClientAccountFeature>
    {
        // Basic Properties
        public virtual int ClientId { get; set; }
        public virtual AccountFeatureEnum AccountFeature { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        //public virtual DateTime SysEndTime { get; set; }

        // Entity References
        public virtual Client Client { get; set; }
        public virtual AccountFeatureInfo AccountFeatureInfo { get; set; }
    }
}