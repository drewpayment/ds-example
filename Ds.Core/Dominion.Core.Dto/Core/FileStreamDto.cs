using System.IO;

namespace Dominion.Core.Dto.Core
{
    public class FileStreamDto
    {
        public Stream FileStream    { get; set; }
        public string FileName      { get; set; }
        public string FileExtension { get; set; }
        public string MimeType      { get; set; }
        public string Id            { get; set; }
    }

    public class FileUrlDto
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public string Id { get; set; }
    }

    public class FileAzureCloudBlobDto
    {
        public AzureCloudBlobDto AzureCloudBlobDto { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string MimeType { get; set; }
        public string Id { get; set; }
    }
}
