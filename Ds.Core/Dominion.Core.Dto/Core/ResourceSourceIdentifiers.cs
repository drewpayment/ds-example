namespace Dominion.Core.Dto.Core
{
    public class ResourceSourceIdentifiers : IResourceSourceIdentifiers
    {
        public int                ResourceId   { get; set; }
        public ResourceSourceType SourceType   { get; set; }
        public string             Source       { get; set; }
        public int?               AzureAccount { get; set; }
    }
}