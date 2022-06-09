using Dominion.Core.Dto.LeaveManagement;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.RequestTimeOffStatus record. 
    /// Status details of a <see cref="TimeOffStatusType"/>.
    /// </summary>
    public partial class TimeOffRequestStatusInfo
    {
        public virtual TimeOffStatusType TimeOffRequestStatusId { get; set; } 
        public virtual string            Description            { get; set; } 
    }
}