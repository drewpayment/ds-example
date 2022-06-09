namespace Dominion.Core.Dto.Core
{
    public interface IMeetingAttendee
    {
        int  MeetingAttendeeId { get; set; }
        int  MeetingId         { get; set; }
        int? UserId            { get; set; }
        int? EmployeeId        { get; set; }
    }
}