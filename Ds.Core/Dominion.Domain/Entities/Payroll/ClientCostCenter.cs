using Dominion.DataModel.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.DataModel.Entities
{
    public partial class ClientCostCenter : Entity<ClientCostCenter>
    {
        public virtual int ClientCostCenterId { get; set; } 
        public virtual int ClientId { get; set; } 
        public virtual string Code { get; set; } 
        public virtual string Description { get; set; } 
        public virtual int? DefaultGlAccountId { get; set; } 
        public virtual bool? IsDefaultGlCostCenter { get; set; } 
        public virtual DateTime? Modified { get; set; } 
        public virtual string ModifiedBy { get; set; } 
        public virtual string GlClassName { get; set; } 
        public virtual bool IsActive { get; set; } 

        //REVERSE NAVIGATION
        public virtual ICollection<PayrollEmployeeOverride> PayrollEmployeeOverride { get; set; } // many-to-one;

        //FOREIGN KEYS
        public virtual Client Client { get; set; } 
    }
}
