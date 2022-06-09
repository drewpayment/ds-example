using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckPayDataHistoryDto
    {
        public int       GenPaycheckPayDataHistoryId { get; set; } 
        public int       GenPaycheckHistoryId        { get; set; } 
        public int?      PayrollPayDataId            { get; set; } 
        public decimal   GrossPay                    { get; set; } 
        public decimal   PartialGrossPay             { get; set; } 
        public decimal   Tips                        { get; set; } 
        public decimal   PartialTips                 { get; set; } 
        public double    PercentOfCheck              { get; set; } 
        public decimal   TotalTax                    { get; set; } 
        public int?      AppliesToPayrollId          { get; set; } 
        public int?      ClientWorkersCompId         { get; set; } 
        public int?      ClientGroupId               { get; set; } 
        public int?      ClientDivisionId            { get; set; } 
        public int?      ClientDepartmentId          { get; set; } 
        public int?      SutaStateClientTaxId        { get; set; } 
        public int?      ClientCostCenterId          { get; set; } 
        public int?      ClientShiftId               { get; set; } 
        public decimal   CheckAmount                 { get; set; } 
        public decimal   NetPay                      { get; set; } 
        public decimal   SocSecWages                 { get; set; } 
        public decimal   MedicareWages               { get; set; } 
        public decimal   FutaWages                   { get; set; } 
        public decimal   MedicareTax                 { get; set; } 
        public decimal   EmployerMedicareTax         { get; set; } 
        public decimal   SocSecTax                   { get; set; } 
        public decimal   EmployerSocSecTax           { get; set; } 
        public decimal   EmployerFutaTax             { get; set; } 
        public decimal   ExemptWages                 { get; set; } 
        public decimal   FlexDeductions              { get; set; } 
        public double    StraightHours               { get; set; } 
        public decimal   StraightPay                 { get; set; } 
        public double    PremiumHours                { get; set; } 
        public decimal   PremiumPay                  { get; set; } 
        public decimal   TipCredits                  { get; set; } 
        public decimal   HireActWages                { get; set; } 
        public decimal   HireActCredit               { get; set; } 
        public int       ClientId                    { get; set; } 
        public int       EmployeeId                  { get; set; } 
        public DateTime  Modified                    { get; set; } 
        public string    ModifiedBy                  { get; set; } 
        public decimal?  CustomGrossPay              { get; set; } 
        public DateTime? PayrollCheckDate            { get; set; } 
        public int?      PayrollId                   { get; set; }

        //REVERSE NAVIGATION
        public IEnumerable<PaycheckTaxHistoryDto> PaycheckTaxHistory { get; set; }
        public IEnumerable<PaycheckDeductionHistoryDto> PaycheckDeductionHistory { get; set; }
        public IEnumerable<PaycheckEarningHistoryDto> PaycheckEarningHistory { get; set; }
        public IEnumerable<PaycheckSutaHistoryDto> PaycheckSutaHistory { get; set; }

        //FOREIGN KEYS
        public ClientCostCenterDto ClientCostCenter { get; set; }
    }
}
