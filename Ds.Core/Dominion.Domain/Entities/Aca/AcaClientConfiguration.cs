using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA per-year settings for client. (Entity for [aca].[ClientConfiguration] table)
    /// </summary>
    public class AcaClientConfiguration : Entity<AcaClientConfiguration>, IHasModifiedData
    {
        public int      ClientId                  { get; set; }
        public short    Year                      { get; set; }
        public bool     IncludeAllEmployeeStatues { get; set; }
        public bool     IsForceAcaEnabled         { get; set; }
        public bool     IsSelfInsured             { get; set; }
        public int      ModifiedBy                { get; set; }
        public DateTime Modified                  { get; set; }
        public string   PlanStartMonth            { get; set; }
        
        public Client Client { get; set; }
    }
}
