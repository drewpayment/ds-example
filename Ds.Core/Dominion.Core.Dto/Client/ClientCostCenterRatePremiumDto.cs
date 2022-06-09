using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Client
{
    public class ClientCostCenterRatePremiumDto
    {
        public int                  ClientCostCenterRatePremiumId { get; set; }
        public int                  ClientCostCenterId            { get; set; }
        public decimal              Rate                          { get; set; }
        public AdditionalAmountType AdditionalAmountType          { get; set; }
    }
}
