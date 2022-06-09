using System;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.ClientFeatures
{
    public class ClientFeaturesAgreement : Entity<ClientFeaturesAgreement>, IHasModifiedOptionalData
    {
        public virtual int ClientFeaturesAgreementID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual int UserID { get; set; }
        public virtual bool Agreed { get; set; }
        public virtual AccountFeatureEnum FeatureOptionID { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
    }
}
