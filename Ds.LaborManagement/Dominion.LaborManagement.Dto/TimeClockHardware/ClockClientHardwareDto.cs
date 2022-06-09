using System;

namespace Dominion.LaborManagement.Dto.TimeClockHardware
{
    public class ClockClientHardwareDto
    {
        public int ClockClientHardwareId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Email { get; set; }
        public string IPAddress { get; set; }
        public DateTime? Modified { get; set; }
        public int ModifiedBy { get; set; }
        public string Number { get; set; }
        public int? ClockClientHardwareFunctionId { get; set; }
        public string SerialNumber { get; set; }
        public string MACAddress { get; set; }
        public string FirmwareVersion { get; set; }
        public bool? IsRental { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? Warranty { get; set; }
        public DateTime? WarrantyEnd { get; set; }
    }
}