using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee.Search
{
    public class NewHireOption : IEmployeeSearchFilterOption
    {
        public DateTime LengthOfService { get; set; }
        public EmployeeSearchFilterType FilterType => EmployeeSearchFilterType.NewHire;

        public int Id => 0;

        public string Name { get; set; }

        public IEmployeeSearchFilterOption ParentOption => null;

        public bool Evaluate(EmployeeSearchDto dto)
        {
            if (!dto.HireDate.HasValue)
            {
                return false;
            }
            var lengthOfService = DateTime.Now.Date.Ticks - dto.HireDate.Value.Date.Ticks;
            return lengthOfService <= LengthOfService.Date.Ticks;
        }
    }
}
