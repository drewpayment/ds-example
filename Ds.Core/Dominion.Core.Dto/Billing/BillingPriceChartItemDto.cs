namespace Dominion.Core.Dto.Billing
{
    public class BillingPriceChartItemDto
    {
        public BillingPriceChartType PriceChartType { get; set; }
        public int?                  FromCount      { get; set; }
        public int?                  ToCount        { get; set; }
        public double?               FlatAmount     { get; set; }
        public double?               PerCountAmount { get; set; }
    }
}
