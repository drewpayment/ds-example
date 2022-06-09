using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffEventSprocDto
    {
        public int?                              RequestTimeOffId                    { get; set; }
        public int                               PolicyId                            { get; set; }
        public string                            PolicyName                          { get; set; }
        public TimeOffStatusType?                TimeOffStatusType                   { get; set; }
        public string                            TimeOffStatusName                   { get; set; }
        public int?                              PendingAwardId                      { get; set; }
        public TimeOffAwardType?                 TimeOffAwardType                    { get; set; }
        public int                               ChangeOrderPriority                 { get; set; }
        public decimal?                          AvailableUnitsBeforeChange          { get; set; }
        public decimal                           AvailableUnitsAfterChange           { get; set; }
        public decimal                           UnitsApplied                        { get; set; }
        public decimal?                          UnitsPerDay                         { get; set; }
        public TimeOffUnitType                   TimeOffUnitType                     { get; set; }
        public int                               EmployeeId                          { get; set; }
        public string                            EmployeeName                        { get; set; }
        public int                               ClientEarningId                     { get; set; }
        public int?                              PayrollId                           { get; set; }
        public DateTime                          PayrollPeriodEndDate                { get; set; }
        public DateTime?                         EventStartDate                      { get; set; }
        public DateTime?                         EventEndDate                        { get; set; }
        public DateTime?                         EventCreatedDate                    { get; set; }
        public int?                              ModifiedByUserId                    { get; set; }
        public bool Display4Decimals { get; set; }
    }
}
