using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.LeaveManagement
{
    public class TimeOffRequestDto
    {
        public int               TimeOffRequestId    { get; set; } 
        public int               ClientAccrualId     { get; set; } 
        public int               EmployeeId          { get; set; } 
        public DateTime          RequestFrom         { get; set; } 
        public DateTime          RequestUntil        { get; set; } 
        public double?           Hours               { get; set; } 
        public string            RequesterNotes      { get; set; } 
        public string            ApproverNotes       { get; set; } 
        public DateTime          Modified            { get; set; } 
        public int               ModifiedBy          { get; set; } 
        public double?           BalanceBeforeApp    { get; set; } 
        public double?           BalanceAfterApp     { get; set; } 
        public DateTime?         OriginalRequestDate { get; set; } 
        public int               ClientId            { get; set; } 
        public TimeOffStatusType Status { get; set; }


        // RELATIONSHIPS
        public ICollection<TimeOffRequestDetailDto> TimeOffRequestDetails { get; set; }
        public ClientAccrualDto ClientAccrual { get; set; }
    }
}
