using Dominion.Core.Dto.Tax;
using Dominion.Taxes.Dto.TaxTypes;

namespace Dominion.Core.Dto.Notification
{
    public class EmployeeChangedW4NotificationDetailDto
    {
        public int           ClientId       { get; set; }
        public int           EmployeeId     { get; set; }
        public string        FirstName      { get; set; }
        public string        LastName       { get; set; }
        public LegacyTaxType TaxTypeChanged { get; set; }
    }
}
