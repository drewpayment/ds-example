namespace Dominion.Core.Dto.Core
{
    public interface IResourceSourceIdentifiers
    {
        int                ResourceId   { get; }
        ResourceSourceType SourceType   { get; }
        string             Source       { get; }
        int?               AzureAccount { get; set; }

    }
}
