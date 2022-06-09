using Newtonsoft.Json;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public class ClientDepartmentOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.Department;
        public int Id => ClientDepartmentId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => Division;
        [JsonIgnore]
        public int ClientDepartmentId { get; set; }
        [JsonIgnore]
        public int ClientDivisionId { get; set; }
        [JsonIgnore]
        public ClientDivisionOption Division { get; set; }


        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}