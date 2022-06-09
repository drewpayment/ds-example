
namespace Dominion.Core.Dto.Tax
{
    /// <summary>
    /// Legacy system's tax types.
    /// <seealso cref="DataServices.dsClientTax.TaxType"/>
    /// </summary>
    public enum LegacyTaxType
    {
        FederalWitholding = 1,
        Futa              = 2,
        StateWitholding   = 3,
        Suta              = 4,
        Disability        = 5,
        LocalResident     = 6,
        LocalNonResident  = 7,
        LocalSchool       = 8,
        OtherTax          = 9,
        EmployerPaid      = 10
    }
}
