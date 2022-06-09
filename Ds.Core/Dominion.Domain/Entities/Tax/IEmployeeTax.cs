using Dominion.Domain.Entities.Clients;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    public interface IEmployeeTax
    {
        int                 EmployeeTaxId             { get; set; }
        int                 EmployeeId                { get; set; }
        Employee.Employee   Employee                  { get; set; }
        int?                ClientTaxId               { get; set; }
        ClientTax           ClientTax                 { get; set; }
        FilingStatus        FilingStatus              { get; set; }
        FilingStatusInfo    FilingStatusInfo          { get; set; }
        byte                NumberOfExemptions        { get; set; }
        byte                NumberOfDependents        { get; set; }
        byte                AdditionalTaxAmountTypeId { get; set; }
        double              AdditionalTaxPercent      { get; set; }
        double              AdditionalTaxAmount       { get; set; }
        bool                IsResident                { get; set; }
        bool                IsActive                  { get; set; }
        string              Description               { get; set; }
        int                 ClientId                  { get; set; }
        Client              Client                    { get; set; }
    }
}
