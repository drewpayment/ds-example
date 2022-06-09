using System;

namespace Dominion.Core.Dto.Accruals
{
    public class ServiceReferencePointDateInfo : IHasServiceReferencePointDateInfo
    {
        public DateTime? HireDate        { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public DateTime? ReHireDate      { get; set; }
        public DateTime? EligibilityDate { get; set; }
    }
}