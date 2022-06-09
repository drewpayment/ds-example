using Newtonsoft.Json;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public class ClientCostCenterOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.CostCenter;
        public int Id => ClientCostCenterId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int ClientCostCenterId { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}