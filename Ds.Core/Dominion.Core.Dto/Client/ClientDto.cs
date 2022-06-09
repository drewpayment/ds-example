using Dominion.Core.Dto.Location;
using Dominion.Core.Dto.Performance.ReviewTemplates;
using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// Defines a class that represents a client with information that can be sent through the API.
    /// </summary>
    public class ClientDto
    {
        public string            ClientStatus      => ClientStatusId?.ToString();
        public string            ClientStatusCode  => ClientStatusId == ClientStatusType.Active ? "A" :
                                                      ClientStatusId == ClientStatusType.InfrequentProcessing ? "A" :
                                                      ClientStatusId == ClientStatusType.Parallel ? "P" :
                                                      ClientStatusId == ClientStatusType.Seasonal ? "A" :
                                                      ClientStatusId == ClientStatusType.Terminated ? "T" :
                                                      ClientStatusId == ClientStatusType.TerminatedWithAccess ? "T" : null;
        public ClientStatusType? ClientStatusId    { get; set; }
        public string            ClientName        { get; set; }
        public int               ClientId          { get; set; }
        public string            ClientCode        { get; set; }
        public bool              IsCurrentlyActive { get; set; }
        public DateTime?         TerminationDate   { get; set; }
        public string            FederalId         { get; set; }
        public bool              IsTaxManagement   { get; set; }
        public int AllowTurboTax { get; set; }

        public DateTime?         AllowAccessUntilDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public StateDto State { get; set; }
        public string PostalCode { get; set; }
        public DateTime? StartDate { get; set; }
        public virtual IEnumerable<ReviewTemplateBaseDto> ReviewTemplates { get; set; }
    }
}
