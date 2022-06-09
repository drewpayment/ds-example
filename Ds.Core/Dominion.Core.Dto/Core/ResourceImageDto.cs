
namespace Dominion.Core.Dto.Core
{
    public class ResourceImageDto
    {
        public int                ResourceId        { get; set; }
        public ResourceSourceType SourceTypeId      { get; set; }
        public string             Source            { get; set; }
        public ImageType          ImageType         { get; set; }
        public ImageSizeType      ImageSizeType     { get; set; }
        public string             AzureResourceName { get; set; }
    }
}
