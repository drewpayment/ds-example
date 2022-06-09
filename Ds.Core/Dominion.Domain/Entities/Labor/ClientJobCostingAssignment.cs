using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Defines a class that represents the [dbo].ClientJobCostingAssignment table.
    /// 
    /// Assignments are relationships between entities and entity groups. (See remarks on ClientJobCosting)
    /// They are more of an implementation detail, but they are a convenient way to lookup a list of entities for a Job Costing.
    /// </summary>
    public class ClientJobCostingAssignment : Entity<ClientJobCostingAssignment>
    {
        public int ClientJobCostingAssignmentId { get; set; }
        public int ClientJobCostingId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public int? ForeignKeyId { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public bool IsEnabled { get; set; }

        public virtual ClientJobCosting ClientJobCosting { get; set; }
        public virtual Client Client { get; set; }
    }
}
