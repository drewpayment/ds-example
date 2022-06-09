using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeGlobalChangePreviewDto
    {
        public int    ClientId                   { get; set; }
        public byte   EmployeeGlobalChangeTypeId { get; set; }
        public byte   Type                       { get; set; }
        public double ChangeFrom                 { get; set; }
        public double ChangeTo                   { get; set; }
        public byte   DeductionField             { get; set; }
        public int    ForeignKeyId               { get; set; }
        public int    ChangeFromId               { get; set; }
        public int    ChangeToId                 { get; set; }
        public string ApplyEffectiveDate         { get; set; }
        public int    EmployeeStatusId           { get; set; }
        public int    EmployeeTypeId             { get; set; }
        public int    ClientDivisionId           { get; set; }
        public int    ClientDepartmentId         { get; set; }
        public int    ClientCostCenterId         { get; set; }
        public int    ClientGroupId              { get; set; }
        public bool   FrequencyWeekly            { get; set; }
        public bool   FrequencyBiWeekly          { get; set; }
        public bool   FrequencySemiMonthly       { get; set; }
        public bool   FrequencyMonthly           { get; set; }
        public bool   FrequencyAltBiWeekly       { get; set; }
        public bool   FrequencyQuarterly         { get; set; }
        public int    JobTitleId                 { get; set; }
        public int    ClientShiftId              { get; set; }
    }
}
