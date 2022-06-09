using System.Collections.Generic;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchResult
    {
        public int  TotalCount  { get; set; }
        public int? PageCount   { get; set; }
        public int? Page        { get; set; }
        public int? PageSize    { get; set; }
        public int  FilterCount { get; set; }
        public IEnumerable<EmployeeSearchDto> Results { get; set; }
        public EmployeeNavInfo Nav { get; set; }
    }
}
