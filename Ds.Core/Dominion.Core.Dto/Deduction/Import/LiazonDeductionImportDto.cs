using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Deduction.Import
{
    /// <summary>
    /// Defines a DTO that represents Deductions loaded from Liazon.
    /// </summary>
    public class LiazonDeductionImportDto
    {
        [Display(Name = "Client")]
        public string Client { get; set; }
        [Display(Name = "EmployeeID")]
        public string Employeeid { get; set; }
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "IsActive")]
        public string DedIsActive { get; set; }
        [Display(Name = "DeductionCode")]
        public string DedCode { get; set; }
        [Display(Name = "DeductionAmount")]
        public string DedAmount { get; set; }
        [Display(Name = "DeductionAmountTypeID")]
        public string DedAmtTypeID { get; set; }
        [Display(Name = "MaxType")]
        public string MaxType { get; set; }
        [Display(Name = "Limit/Balance")]
        public string LimitBalance { get; set; }
    }
}
