namespace Dominion.Core.Dto.App
{
    public class ApplicationResourceDto
    {
        public int                     ResourceId            { get; set; }
        public string                  Name                  { get; set; }
        public string                  RouteUrl              { get; set; }
        public ApplicationSourceType   ApplicationSourceType { get; set; }
        public ApplicationResourceType ResourceType          { get; set; }
    }
}