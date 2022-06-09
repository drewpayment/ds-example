using System.Collections.Generic;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeTaxAdminDto
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public EmployeeTaxSetupDto FederalTax { get; set; }
        public IEnumerable<EmployeeNonFederalTaxDto> NonFederalTaxes { get; set; }
        public EmployeeTaxGeneralInfoDto GeneralInfo { get; set; }
    }
}
