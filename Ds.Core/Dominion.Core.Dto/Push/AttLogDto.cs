using System;

namespace Dominion.Core.Dto.Push
{
    public class AttLogDto
    {
        public int       Id        { get; set; }
        public string    DevSn     { get; set; }
        public string    Pin       { get; set; }
        public DateTime? AttTime   { get; set; }
        public string    Status    { get; set; }
        public string    Verify    { get; set; }
        public string    WorkCode  { get; set; }
        public string    Reserved1 { get; set; }
        public string    Reserved2 { get; set; }
        public int?      JobCode1  { get; set; }
        public int?      JobCode2  { get; set; }
        public int?      JobCode3  { get; set; }
        public int?      JobCode4  { get; set; }
        public int?      JobCode5  { get; set; }
        public int?      JobCode6  { get; set; }
        public int?      TipCode1  { get; set; }
        public int?      TipCode2  { get; set; }
        public int?      TipCode3  { get; set; }
    }
}
