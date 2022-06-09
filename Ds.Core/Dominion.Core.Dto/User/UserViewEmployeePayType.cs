namespace Dominion.Core.Dto.User
{
    /// <summary>
    /// Pay types that a user may be allowed to view.
    /// </summary>
    /// <remarks>
    /// The numeric values are important because they are persisted in the db.
    /// </remarks>
    public enum UserViewEmployeePayType : byte
    {
        Hourly = 1, 
        Salary = 2, 
        Both = 3, 
        None = 4
    }
}