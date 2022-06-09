namespace Dominion.Core.Dto.Client
{
    /// <summary>
    /// Enum representation of the SecurityLevels for company Resources.
    /// </summary>
    public enum CompanyResourceSecurityLevel
    {
        SystemAdminOnly  = 1,
        CompanyAdminHigher  = 2,
        AllEmployees = 3,
        SupervisorsAndHigher =4
        
    }
}
