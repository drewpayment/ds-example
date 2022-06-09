using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Domain.Entities.Payroll
{
    /// <summary>
    /// Identities a <see cref="TaxType"/> that is never withheld by the given <see cref="ClientEarning"/>.
    /// </summary>
    public partial class ClientEarningWithholdingOverride : Entity<ClientEarningWithholdingOverride>
    {
        public virtual int     ClientEarningId { get; set; } 
        public virtual TaxType TaxType         { get; set; } 

        public virtual ClientEarning ClientEarning      { get; set; } 
        public virtual TaxTypeInfo   TaxTypeInformation { get; set; }
    }
}
