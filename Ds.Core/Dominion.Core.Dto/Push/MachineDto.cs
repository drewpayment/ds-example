using System;

namespace Dominion.Core.Dto.Push
{
    public class MachineDto
    {
        public int Id { get; set; }
        public string DevSn { get; set; }
        public string DevName { get; set; }
        public string AttLogStamp { get; set; }
        public string OperLogStamp { get; set; }
        public string AttPhotoStamp { get; set; }
        public string ErrorDelay { get; set; }
        public string Delay { get; set; }
        public string TransFlag { get; set; }
        public string Realtime { get; set; }
        public string TransInterval { get; set; }
        public string TransTimes { get; set; }
        public string Encrypt { get; set; }
        public DateTime? LastRequestTime { get; set; }
        public string DevIp { get; set; }
        public string DevMac { get; set; }
        public string DevFpVersion { get; set; }
        public string DevFirmwareVersion { get; set; }
        public int? UserCount { get; set; }
        public int? AttCount { get; set; }
        public int? FpCount { get; set; }
        public string TimeZone { get; set; }
        public int? Timeout { get; set; }
        public int? SyncTime { get; set; }
    }
}