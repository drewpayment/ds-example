using System.Collections.Generic;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSelectionDto
    {
        public EmployeeSearchQueryOptions SearchOptions { get; set; }
        public bool IncludeAllSearchResults { get; set; }
        public IEnumerable<int> Exclude { get; set; }
        public IEnumerable<int> Include { get; set; }
    }
}