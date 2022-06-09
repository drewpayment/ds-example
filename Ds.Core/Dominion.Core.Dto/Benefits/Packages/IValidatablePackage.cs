namespace Dominion.Core.Dto.Benefits.Packages
{
    /// <summary>
    /// Properties required to validate a benefit package.
    /// </summary>
    public interface IValidatablePackage
    {
        int    BenefitPackageId { get; }
        int    ClientId         { get; }
        string Name             { get; }
    }
}