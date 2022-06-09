using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class InsertEmployeeBankDeductionDto
    {
        public int AccountType { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public bool isPreNote { get; set; }
        public int EmployeeID { get; set; }
        public int ClientID { get; set; }
        public int ModifiedBy { get; set; }
        public int EmployeeDeductionID { get; set; }
    }
}
