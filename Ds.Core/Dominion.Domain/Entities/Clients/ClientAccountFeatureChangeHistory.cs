using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.Clients
{
    /// <summary>
    /// Client Feature Entity
    /// </summary>
    public class ClientAccountFeatureChangeHistory : Entity<ClientAccountFeatureChangeHistory>
    {
        // Basic Properties
        public virtual int ClientId { get; set; }
        public virtual AccountFeatureEnum AccountFeature { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual DateTime SysEndTime { get; set; }
    }
}