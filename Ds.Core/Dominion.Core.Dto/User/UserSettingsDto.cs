using System;

using Dominion.Utility.Dto;

namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// This is a container class for generic user data that is intended to service user creation
    /// and update actions.
    /// </summary>
    public class UserSettingsDto : DtoObject
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public int EmployeeId { get; set; }
        public int LastClientId { get; set; }
        public bool ChangeRequestRequired { get; set; }
    }
}