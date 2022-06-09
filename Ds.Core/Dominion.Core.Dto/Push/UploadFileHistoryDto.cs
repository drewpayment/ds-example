using System;

namespace Dominion.Core.Dto.Push
{
    public class UploadFileHistoryDto
    {
        public int       Id              { get; set; }
        public string    FileName        { get; set; }
        public string    DestinationPath { get; set; }
        public DateTime? CreateTime      { get; set; }
    }
}
