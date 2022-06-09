using System;

namespace Dominion.Core.Dto.Benefits.Packages
{
    /// <summary>
    /// Benefit Package information.
    /// </summary>
    [Serializable]
    public class BenefitPackageDto : IValidatablePackage
    {
        public int    BenefitPackageId { get; set; }
        public int    ClientId         { get; set; }
        public string Name             { get; set; }
        public int    EmployeeCount    { get; set; }
        public int    PlanCount        { get; set; }
    }
}
