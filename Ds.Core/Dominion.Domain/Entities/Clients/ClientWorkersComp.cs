using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientWorkersComp : Entity<ClientWorkersComp>, IHasModifiedUserNameData
    {
        public virtual int      ClientWorkersCompId { get; set; }
        public virtual int      ClientId            { get; set; }
        public virtual string   Class               { get; set; }
        public virtual string   Description         { get; set; }
        public virtual DateTime EffectiveDate       { get; set; }
        public virtual double   Rate                { get; set; }
        public virtual DateTime Modified            { get; set; }
        public virtual string   ModifiedBy          { get; set; }
        public virtual bool     IsActive            { get; set; }
        public virtual Client   Client              { get; set; }
    }
}
