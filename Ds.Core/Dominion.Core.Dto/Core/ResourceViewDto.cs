using System;

namespace Dominion.Core.Dto.Core
{
    [Serializable]
    public class ResourceViewDto
    {
        public int                ResourceId  { get; set; }
        public string             Name        { get; set; }
        public ResourceSourceType SourceType  { get; set; }
        public string             ResourceUrl { get; set; }
    }
}
