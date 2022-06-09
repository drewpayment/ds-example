using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeeAvatar : Entity<EmployeeAvatar>,  IHasModifiedData
    {
        public int EmployeeAvatarId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string AvatarColor { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}