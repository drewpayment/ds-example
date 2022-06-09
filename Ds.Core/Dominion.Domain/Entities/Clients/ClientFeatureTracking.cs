using System;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientFeatureTracking : Entity<ClientFeatureTracking>, IHasModifiedData
    {
        public virtual int ClientFeatureTrackingId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual AccountFeatureEnum FeatureOptionId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
