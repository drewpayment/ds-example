using System.Collections.Generic;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeChangesFormattedDto
    {
        public string tFriendlyDesc { get; set; }
        public IEnumerable<EmployeeChangesDto> changedItem { get; set; }
    }
}
