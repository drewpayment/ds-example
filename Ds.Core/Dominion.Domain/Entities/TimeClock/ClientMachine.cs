using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Push;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.TimeClock
{
    public class ClientMachine : Entity<ClientMachine>, IHasModifiedData
    {
        public virtual int       ClientMachineId { get; set; }
        public virtual int       ClientId        { get; set; }
        public virtual string    Description     { get; set; }
        public virtual DateTime  Modified        { get; set; }
        public virtual int       ModifiedBy      { get; set; }
        public virtual bool      IsRental        { get; set; }
        public virtual DateTime? PurchaseDate    { get; set; }
        public virtual DateTime? Warranty        { get; set; }
        public virtual DateTime? WarrantyEnd     { get; set; }
        public virtual int       PushMachineId   { get; set; }

        public virtual Client      Client      { get; set; }
        public virtual PushMachine PushMachine { get; set; }
    }
}
