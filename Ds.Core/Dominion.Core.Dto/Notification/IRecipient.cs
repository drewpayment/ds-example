namespace Dominion.Core.Dto.Notification
{
    public interface IRecipient
    {
        int? UserId      { get; }
        int? EmployeeId  { get; }
        int? ApplicantId { get; }
    }
}
