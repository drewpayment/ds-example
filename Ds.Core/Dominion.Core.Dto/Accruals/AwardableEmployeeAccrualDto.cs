using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.LeaveManagement;

namespace Dominion.Core.Dto.Accruals
{
    public class AwardableEmployeeAccrualDto
    {
        public int                             EmployeeAccrualId                       { get; set; }
        public int                             EmployeeId                              { get; set; }
        public int                             ClientAccrualId                         { get; set; }
        public bool?                           AllowScheduledAwards                    { get; set; }
        public bool?                           IsActive                                { get; set; }
        public string                          Description                             { get; set; }
        public string                          ClientEarningId                         { get; set; }
        public bool                            CombineByEarnings                       { get; set; }
        public bool?                           UseCheckDates                           { get; set; }
        public ServiceReferencePointType       ServiceReferencePointId                 { get; set; }
        public int                             AccrualBalanceOptionId                  { get; set; }
        public bool                            Display4Decimals                        { get; set; }
        public bool                            ShowPreviewMessages                     { get; set; }
        public ServiceReferencePointType?      ProratedServiceReferencePointOverrideId { get; set; }
        public ProratedAccrualWhenToAwardType? ProratedAccrualWhenToAwardTypeId        { get; set; }
        public DateTime?                       HireDate                                { get; set; }
        public DateTime?                       RehireDate                              { get; set; }
        public DateTime?                       AnniversaryDate                         { get; set; }
        public DateTime?                       EligibilityDate                         { get; set; }
    }
}
