using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Tax
{
    public class EmployeeTaxCostCenter :
        Entity<EmployeeTaxCostCenter>,
        IHasEmployeeId,
        IHasModifiedData
    {
        public int EmployeeTaxId { get; set; }
        public int ClientCostCenterId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        public virtual EmployeeTax EmployeeTax { get; set; }
        public virtual ClientCostCenter ClientCostCenter { get; set; }
    }
}
