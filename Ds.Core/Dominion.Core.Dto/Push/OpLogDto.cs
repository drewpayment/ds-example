using System;

namespace Dominion.Core.Dto.Push
{
    public class OpLogDto
    {
        public int       Id         { get; set; }
        public string    OpId       { get; set; }
        public string    AdminId    { get; set; }
        public DateTime? HappenTime { get; set; }
        public string    Object1    { get; set; }
        public string    Object2    { get; set; }
        public string    Object3    { get; set; }
        public string    Object4    { get; set; }
    }
}
