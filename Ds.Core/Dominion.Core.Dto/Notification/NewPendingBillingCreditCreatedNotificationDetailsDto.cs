using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    [Serializable]
    public class NewPendingBillingCreditCreatedNotificationDetailsDto
    {
        public int RequestedByUserId { get; set; }
        public string RequestedByUsername { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestedByFirstName { get; set; }
        public string RequestedByLastName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
