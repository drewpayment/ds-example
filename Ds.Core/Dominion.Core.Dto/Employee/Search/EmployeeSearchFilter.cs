using System.Collections.Generic;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchFilter
    {
        public EmployeeSearchFilterType                 FilterType    { get; set; }
        public string                                   Description   { get; set; }
        public IEnumerable<IEmployeeSearchFilterOption> FilterOptions { get; set; }
    }
}