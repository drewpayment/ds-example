using System;

namespace Dominion.Core.Dto.Core
{
    public class LocalFileResourceInfoDto
    {
        public int       ResourceId   { get; set; }
        public int?      ClientId     { get; set; }
        public int?      EmployeeId   { get; set; }
        public string    Filename     { get; set; }
        public string    Name         { get; set; }
        public DateTime? AddedDate    { get; set; }
        public int?      AddedById    { get; set; }
    }
}
