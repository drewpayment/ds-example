using System;
using Dominion.Core.Dto.Push;

namespace Dominion.Core.Dto.TimeClock
{
    public class ClientMachineDto
    {
        public int       ClientMachineId { get; set; }
        public int       ClientId        { get; set; }
        public string    Description     { get; set; }
        public DateTime  Modified        { get; set; }
        public int       ModifiedBy      { get; set; }
        public bool      IsRental        { get; set; }
        public DateTime? PurchaseDate    { get; set; }
        public DateTime? Warranty        { get; set; }
        public DateTime? WarrantyEnd     { get; set; }
        public int       PushMachineId   { get; set; }

        public MachineDto PushMachine { get; set; }
    }
}
