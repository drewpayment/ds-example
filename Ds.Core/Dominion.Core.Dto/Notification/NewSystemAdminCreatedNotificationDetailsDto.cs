using System;

namespace Dominion.Core.Dto.Notification
{
    [Serializable]
    public class NewSystemAdminCreatedNotificationDetailsDto
    {
        public int? AuthUserId { get; set; }
        public int DsUserId { get; set; }
        public int AddedByUserId { get; set; }
        public string AddedByUsername { get; set; }
        public DateTime DateAdded { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
