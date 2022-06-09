using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.TimeClock
{
    public partial class ClockClientHardware : Entity<ClockClientHardware>, IHasModifiedData
    {
        public virtual int ClockClientHardwareId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual string Model { get; set; }
        public virtual string Email { get; set; }
        public virtual string IPAddress { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string Number { get; set; }
        public virtual int? ClockClientHardwareFunctionId { get; set; }
        public virtual string SerialNumber { get; set; }
        public virtual string MACAddress { get; set; }
        public virtual string FirmwareVersion { get; set; }
        public virtual bool IsRental { get; set; }
        public virtual DateTime? PurchaseDate { get; set; }
        public virtual DateTime? Warranty { get; set; }
        public virtual DateTime? WarrantyEnd { get; set; }

        public ClockClientHardware()
        {
        }
    }
}