using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

public partial class EmployeeClientRateCostCenter : Entity<EmployeeClientRateCostCenter>, IHasModifiedOptionalData
{
    public virtual int              EmployeeClientRateCostCenterId { get; set; }
    public virtual int              EmployeeId                     { get; set; }
    public virtual int              ClientRateId                   { get; set; }
    public virtual int              ClientCostCenterId             { get; set; }
    public virtual DateTime?        Modified                       { get; set; }
    public virtual int?             ModifiedBy                     { get; set; }
    public virtual int              ClientId                       { get; set; }
    public virtual Employee         Employee                       { get; set; }
    public virtual ClientRate       ClientRate                     { get; set; }
    public virtual ClientCostCenter ClientCostCenter               { get; set; }
    public virtual Client           Client                         { get; set; }
}
