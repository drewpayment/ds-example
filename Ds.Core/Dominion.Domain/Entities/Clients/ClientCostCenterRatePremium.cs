using System;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public class ClientCostCenterRatePremium : Entity<ClientCostCenterRatePremium>, IHasModifiedData
    {
        public int                     ClientCostCenterRatePremiumId { get; set; } 
        public int                     ClientId                      { get; set; } 
        public int                     ClientCostCenterId            { get; set; } 
        public decimal                 Rate                          { get; set; } 
        public AdditionalAmountType    AdditionalAmountTypeId        { get; set; } 
        public DateTime                Modified                      { get; set; } 
        public int                     ModifiedBy                    { get; set; } 

        public virtual Client Client { get; set; }
        public virtual ClientCostCenter ClientCostCenter { get; set; }
        public virtual AdditionalAmountTypeInfo AdditionalAmountTypeInfo { get; set; }
    }
}
