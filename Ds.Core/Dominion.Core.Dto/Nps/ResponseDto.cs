using System;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Nps
{
    public class ResponseDto
    {
        public int ResponseId { get; set; }
        public int QuestionId { get; set; }
        // User ID of the person that completed the NPS survey
        public int UserId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public UserType UserTypeId { get; set; }
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public DateTime ResponseDate { get; set; }
        public byte Score { get; set; }
        public string Feedback { get; set; }
        public bool? IsResolved { get; set; }
        public int? ResolvedByUserId {get; set;}
        public string ResolvedByUserName { get; set; }
        public DateTime? ResolvedDate { get; set; }
    }
}
