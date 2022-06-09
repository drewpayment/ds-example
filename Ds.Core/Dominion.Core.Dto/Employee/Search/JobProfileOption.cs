using Newtonsoft.Json;
using System.Linq;

namespace Dominion.Core.Dto.Employee.Search
{
    public class JobProfileOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.JobProfile;
        public int Id => JobProfileId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int JobProfileId { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}