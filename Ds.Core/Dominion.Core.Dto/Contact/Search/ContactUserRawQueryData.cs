using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Contact.Search
{
    public class ContactUserRawQueryData
    {
        public int      UserId     { get; set; }
        public int?      AuthUserId { get; set; }
        public UserType UserType   { get; set; }
        public int?     EmployeeId { get; set; }
        public string   FirstName  { get; set; }
        public string   LastName   { get; set; }
        public bool     IsUserDisabled  { get; set; }
        public bool TimeclockAppOnly { get; set; }
    }
}