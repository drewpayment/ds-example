using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class UpdateEmployeeDeductionDto
    {
        public int EmployeeDeductionID { get; set; }
        public int ClientDeductionID { get; set; }
        public int EmployeeBankID { get; set; }
        public int EmployeeBondID { get; set; }
        public int EmployeeID { get; set; }
        public int ClientPlanID { get; set; }
        public double Amount { get; set; }
        public int DeductionAmountTypeID { get; set; }
        public double Max { get; set; }
        public int MaxType { get; set; }
        public double TotalMax { get; set; }
        public int ClientVendorID { get; set; }
        public string AdditionalInfo { get; set; }
        public bool isActive { get; set; }
        public int ModifiedBy { get; set; }
        public int ClientCostCenterID { get; set; }
    }
}
