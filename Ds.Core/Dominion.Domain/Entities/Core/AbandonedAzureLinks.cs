using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Core
{
    public class AbandonedAzureLinks : Entity<AbandonedAzureLinks>, IHasModifiedData
    {
        public virtual int      AbandonedAzureLinkId     { get; set; }
        public int              ClientId                 { get; set; } 
        public int?             EmployeeId               { get; set; }
        public string           FileName                 { get; set; } 
        public string           Source                   { get; set; } 
        public virtual DateTime Modified                 { get; set; }
        public virtual int      ModifiedBy               { get; set; }
    }
}
