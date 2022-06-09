using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class BasicEmployeeDto
    {
        public string    FirstName      { get; set; }
        public string    LastName       { get; set; }
        public string    EmployeeNumber { get; set; }
        public DateTime?  HireDate       { get; set; }
        public DateTime? RehireDate     { get; set; }
    }
}
