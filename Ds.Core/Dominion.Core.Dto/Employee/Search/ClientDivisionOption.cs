using Newtonsoft.Json;
using System;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public class ClientDivisionOption : IEmployeeSearchFilterOption
    {

        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.Division;
        public int Id => ClientDivisionId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int ClientDivisionId { get; set; }

        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}