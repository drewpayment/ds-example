using System;

namespace Dominion.Core.Dto.Push
{
    public class DeviceCmdDto
    {
        public int       Id           { get; set; }
        public string    DevSn        { get; set; }
        public string    Content      { get; set; }
        public DateTime? CommitTime   { get; set; }
        public DateTime? TransTime    { get; set; }
        public DateTime? ResponseTime { get; set; }
        public string    ReturnValue  { get; set; }
    }
}
