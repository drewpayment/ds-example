using Newtonsoft.Json;

namespace Dominion.Core.Dto.Employee.Search
{
    public class PayTypeOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.PayType;
        public int Id => (int)PayType;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public PayType PayType { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}