using System.Collections.Generic;

namespace Dominion.Core.Dto.Core
{
    public class EmployeeProfileImageDto
    {
        public int                          EmployeeId       { get; set; }
        public string                       EmployeeGuid     { get; set; }
        public int                          ClientId         { get; set; }
        public string                       ClientGuid       { get; set; }
        public string                       SasToken         { get; set; }
        public IEnumerable<ResourceImageDto> ProfileImageInfo { get; set; }
    }
}
