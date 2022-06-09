using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core.Search;
using Dominion.Utility.Query;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeSearchQueryOptions : IHasPaginationPage
    {
        public int?                      ClientId       { get; set; }
        public int?                      EmployeeId     { get; set; }
        public int?                      Page           { get; set; }
        public int?                      PageSize       { get; set; }
        public string                    SearchText     { get; set; }
        public EmployeeSearchFilterType? SortBy         { get; set; }
        public SortDirection?            SortDirection  { get; set; }
        public bool                      IsExcludeTemps { get; set; }
        public bool                      IsActiveOnly   { get; set; }
        public IEnumerable<IEmployeeSearchFilterOption> Filters { get; set; }
        public Func<EmployeeSearchDto, bool> AdditionalFilter { get; set; }
    }
}