using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Payroll
{
    public interface IClientPlanEntity
    {
        int ClientPlanId { get; set; }
        int ClientId { get; set; }
        string Description { get; set; }
        double Amount { get; set; }
        int DeductionAmountTypeId { get; set; }
        int ModifiedBy { get; set; }
        bool Inactive { get; set; }
    }
}
