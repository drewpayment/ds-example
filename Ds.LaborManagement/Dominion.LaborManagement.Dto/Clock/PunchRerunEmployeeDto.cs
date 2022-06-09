using System.Collections.Generic;
using Dominion.Core.Dto.Contact.Search;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchRerunEmployeeDto : ContactSearchDto
    {
        public string EmployeeNumber { get; set; }
        public IEnumerable<PunchRerunInfoDto> Punches { get; set; }
    }
}