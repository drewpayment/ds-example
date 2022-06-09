using System;

namespace Dominion.Core.Dto.Notification
{
    public class NewCompanyAdminCreatedNotificationDetailDto
    {
        public int? AuthUserId { get; set; }
        public int DsUserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int AddedByUserId { get; set; }
        public string AddedByUsername { get; set; }
        public DateTime DateAdded { get; set; }
    }
}