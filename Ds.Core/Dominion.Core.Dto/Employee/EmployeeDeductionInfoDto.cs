using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeDeductionInfoDto
    {
        public int? EmployeeDeductionID { get; set; }
        public int? ClientDeductionID { get; set; }
        public int? EmployeeBankID { get; set; }
        public int? EmployeeBondID { get; set; }
        public int? AccountType { get; set; }
        public string NumberPrefix { get; set; }
        public string AccountTypeDescription { get; set; }
        public int? ClientPlanID { get; set; }
        public double? Amount { get; set; }
        public int? DeductionAmountTypeID { get; set; }
        public double? Max { get; set; }
        public int? MaxType { get; set; }
        public string MaxTypeDescription { get; set; }
        public double? TotalMax { get; set; }
        public int? ClientVendorID { get; set; }
        public string AdditionalInfo { get; set; }
        public string Code { get; set; }
        public string Deduction { get; set; }
        public string EmployeeBank { get; set; }
        public string EmployeeBond { get; set; }
        public string Plan { get; set; }
        public string AmountType { get; set; }
        public string DescriptionCode { get; set; }
        public string Vendor { get; set; }
        public bool isActive { get; set; }
        public int? GroupSequence { get; set; }
        public string EarningDescription { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
        public bool isPreNote { get; set; }
        public int? ClientCostCenterID { get; set; }
        public byte? SubSequenceNum { get; set; }
    }
}
