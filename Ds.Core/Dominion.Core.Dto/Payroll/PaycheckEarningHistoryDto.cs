using Dominion.Core.Dto.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckEarningHistoryDto
    {
        public int       GenPaycheckEarningHistoryId { get; set; } 
        public int       GenPaycheckPayDataHistoryId { get; set; } 
        public int       ClientEarningId             { get; set; } 
        public double    EarningPercent              { get; set; } 
        public bool      IsTips                      { get; set; } 
        public byte      CalcShiftPremium            { get; set; } 
        public double    AdditionalAmount            { get; set; } 
        public int       AdditionalAmountTypeId      { get; set; } 
        public int       Destination                 { get; set; } 
        public double?   Hours                       { get; set; } 
        public decimal?  Amount                      { get; set; } 
        public decimal   TotalAmount                 { get; set; } 
        public int?      ClientRateId                { get; set; } 
        public double?   Rate                        { get; set; } 
        public ClientEarningCategory? ClientEarningCategoryId     { get; set; } 
        public bool      IsIncludeInDeductions       { get; set; } 
        public int       ClientId                    { get; set; } 
        public int       EmployeeId                  { get; set; } 
        public bool      IsShiftPremium              { get; set; } 
        public DateTime  Modified                    { get; set; } 
        public string    ModifiedBy                  { get; set; } 
        public double    ActualHours                 { get; set; } 
        public double    ActualTotalAmount           { get; set; } 
        public bool?     IsServiceChargeTips         { get; set; } 
        public double    ActualAmount                { get; set; } 
        public DateTime? PayrollCheckDate            { get; set; } 
        public int?      PayrollId                   { get; set; } 
        public int?      PaycheckId                  { get; set; } 

        //FOREIGN KEYS
        public PaycheckPayDataHistoryDto PaycheckPayDataHistory { get; set; } 
        public ClientEarningDto          ClientEarning          { get; set; }
        public ClientRateDto             ClientRate             { get; set; }
        public Client.ClientDto          Client                 { get; set; }
        public PayrollDto                Payroll                { get; set; }
        public PaycheckHistoryDto        PaycheckHistory        { get; set; }
    }
}
