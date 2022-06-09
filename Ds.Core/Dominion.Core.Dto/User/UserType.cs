namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// User types available within the legacy system.
    /// </summary>
    public enum UserType
    {
        Undefined = 0, 
        SystemAdmin = 1, 
        CompanyAdmin = 2, 
        Employee = 3, 
        Supervisor = 4, 
        Applicant = 5
    }
}