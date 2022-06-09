using Newtonsoft.Json;

namespace Dominion.Core.Dto.Employee.Search
{
    public class TimePolicyOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.TimePolicy;
        public int Id => TimePolicyId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int TimePolicyId { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}