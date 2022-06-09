using System;

namespace Dominion.Core.Dto.Accruals
{
    public interface IHasServiceReferencePointDateInfo
    {
        DateTime? HireDate        { get; }
        DateTime? AnniversaryDate { get; }
        DateTime? ReHireDate      { get; }
        DateTime? EligibilityDate { get; }
    }
}
