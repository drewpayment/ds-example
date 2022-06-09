using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee.Search
{
    public class CompetencyModelOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.CompetencyModel;
        public int Id => CompetencyModelId;
        public string Name { get; set; }
        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int CompetencyModelId { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}
