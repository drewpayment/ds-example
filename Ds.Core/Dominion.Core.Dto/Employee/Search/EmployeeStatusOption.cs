using Newtonsoft.Json;

namespace Dominion.Core.Dto.Employee.Search
{
    public class EmployeeStatusOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.EmployeeStatus;
        public int Id => (int)EmployeeStatus;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public EmployeeStatusType EmployeeStatus { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}