using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee.Search
{
    public class DirectSupervisorOption : IEmployeeSearchFilterOption
    {
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.DirectSupervisor;

        public int Id => SupervisorUserId;

        public string Name => SupervisorName;

        public IEmployeeSearchFilterOption ParentOption => null;
        [JsonIgnore]
        public int SupervisorUserId { get; set; }
        [JsonIgnore]
        public string SupervisorName { get; set; }
        bool IEmployeeSearchFilterOption.Evaluate(EmployeeSearchDto dto)
        {
            return FilterFns.EvaluateById(dto, this);
        }
    }
}
