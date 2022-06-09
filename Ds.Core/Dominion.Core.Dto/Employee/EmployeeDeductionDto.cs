using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeDeductionDto
    {
        public int EmployeeId { get; set; }
        public int? ClientDeductionId { get; set; }
        public int EmployeeDeductionId { get; set; }
        public int ClientId { get; set; }
        public ClientDeductionCategoryType? ClientDeductionCategoryId { get; set; }
        public double? Amount { get; set; }
        public EmployeeDeductionAmountType? DeductionAmountTypeId { get; set; }
        public double? Max { get; set; }
        public EmployeeDeductionMaxType? MaxType { get; set; }
        public double? TotalMax { get; set; }
        public int? ClientPlanId { get; set; }
        public int? ClientVendorId { get; set; }
        public int? EmployeeBankId { get; set; }
        public int? EmployeeBondId { get; set; }
        public string AdditionalInfo { get; set; }
        public byte? SubSequenceNum { get; set; }
        public bool IsActive { get; set; }
        public DateTime? Modified { get; set; }
        public int? ModifiedBy { get; set; }

        public EmployeeDeductionDto ShallowCopy()
        {
            return (EmployeeDeductionDto) this.MemberwiseClone();
        }
    }
}
