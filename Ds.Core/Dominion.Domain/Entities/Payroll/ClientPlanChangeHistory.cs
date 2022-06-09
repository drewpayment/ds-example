using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class ClientPlanChangeHistory : Entity<ClientPlanChangeHistory>, IHasChangeHistoryDataWithEnum, IClientPlanEntity
    {
        public virtual int ClientPlanId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual double Amount { get; set; }
        public virtual int DeductionAmountTypeId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual bool Inactive { get; set; }
        public int ChangeId { get; set; }
        public ChangeModeType ChangeMode { get; set; }
        public DateTime ChangeDate { get; set; }
    }
}
