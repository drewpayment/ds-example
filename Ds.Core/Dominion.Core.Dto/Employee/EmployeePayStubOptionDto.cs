using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public class EmployeePayStubOptionDto
    {
        public int EmployeeId { get; set; }
        public int? PayStubOption { get; set; }
    }
}