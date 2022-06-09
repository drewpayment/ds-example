using Newtonsoft.Json;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public class ClientGroupOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.Group;
        public int Id => ClientGroupId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int ClientGroupId { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}