using System;

namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public class ContactBusinessIdentifiersDto2
    {
        public int ClientId { get; set; }
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }
    }
}