namespace Dominion.Core.Dto.Tax
{
    public enum WithholdingOption : byte
    {
        Additional = 0,
        Override = 1,
        StopTax = 2,
        StopTaxAndWages = 3
    }
}
