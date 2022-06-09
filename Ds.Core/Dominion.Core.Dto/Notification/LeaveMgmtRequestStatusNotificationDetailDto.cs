using System;

namespace Dominion.Core.Dto.Notification
{
    [Serializable]
    public class LeaveMgmtRequestNotificationDetailDto
    {
        public int       ClientId         { get; set; }
        public int       EmployeeId       { get; set; }
        public string    EmployeeName     { get; set; }
        public int       RequestTimeOffId { get; set; }
        public string    PolicyDesc       { get; set; }
        public DateTime  RequestFrom      { get; set; }
        public DateTime  RequestUntil     { get; set; }
        public string    FromTime         { get; set; }
        public string    ToTime           { get; set; }
        public string    Status           { get; set; }
        
    }
}
