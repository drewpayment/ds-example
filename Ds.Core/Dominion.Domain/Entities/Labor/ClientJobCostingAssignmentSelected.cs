using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Defines a class that represents the [dbo].ClientJobCostingAssignmentSelected table.
    /// 
    /// Selections are relationships between an assignment from one entity group and a assignment from another entity group. (See remarks on ClientJobCosting)
    /// tl;dr Selections limit associations in entity groups.
    /// Essentially, they allow admins to select which assignments they want to be associatable
    /// in a group underneath. For example, if a client has three Job Costings: Location, Department, & Cost Center, then
    /// selections at the Location level can be made for Department and Cost Center. On the other hand, the Department level
    /// can only make selections for the cost center level.
    /// </summary>
    public class ClientJobCostingAssignmentSelected : Entity<ClientJobCostingAssignmentSelected>
    {
        public int ClientJobCostingAssignmentSelectedId { get; set; }
        /// <summary>
        /// The ID of the assignment that this selection relates from.
        /// i.e. The first assignment.
        /// </summary>
        public int ClientJobCostingAssignmentId { get; set; }
        
        /// <summary>
        /// The ID of the other assignment that this selection relates to when the other assignment is for a custom entity group type.
        /// Otherwise, if the other assignment is to a non-custom entity group type, then this is null.
        /// </summary>
        public int? ClientJobCostingAssignmentId_Selected { get; set; }
        public int ClientId { get; set; }
        /// <summary>
        /// The ID of the job costing that this selection is for. Only applicable for non-custom entity group types.
        /// Null if the entity group type is custom.
        /// </summary>
        public int? ClientJobCostingId_Selected { get; set; }
        /// <summary>
        /// The ID of the actual entity that this selection is for. Only applicable for non-custom entity group types.
        /// Null if the entity group type is custom.
        /// </summary>
        public int? ForeignKeyId_Selected { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        /// <summary>
        /// Whether this selection is enabled. If not enabled, then it is treated as deleted.
        /// </summary>
        public bool IsEnabled { get; set; }

        public virtual ClientJobCostingAssignment ClientJobCostingAssignment { get; set; }
        public virtual ClientJobCostingAssignment ClientJobCostingAssignment_Selected { get; set; }
        public virtual ClientJobCosting ClientJobCosting_Selected { get; set; }
        public virtual Client Client { get; set; }
    }
}
