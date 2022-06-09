namespace Dominion.Core.Dto.Notification
{
    public class EmployeeCompletedOnboardingNotificationDetailDto
    {
        public int    ClientId       { get; set; }
        public int    EmployeeId     { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName      { get; set; }
        public string MiddleInitial  { get; set; }
        public string LastName       { get; set; }
        
    }
}
