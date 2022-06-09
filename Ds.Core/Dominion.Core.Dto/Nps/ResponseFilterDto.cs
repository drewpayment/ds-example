using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.User;

namespace Dominion.Core.Dto.Nps
{
    public class ResponseFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? ClientOrganizationId { get; set; } //ClientRelationId
        public int? ClientId { get; set; }
        public ClientStatusType? ClientStatus { get; set; }
        public bool? ResolutionStatus { get; set; }
        public IEnumerable<UserType> UserTypes { get; set; }
        public IEnumerable<ResponseType> ResponseTypes { get; set; }
        public bool? IsResolved { get; set; }
        public bool? HideResponsesWithoutFeedback { get; set; }
        public string SearchFeedback { get; set;}
    }
}