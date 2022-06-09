namespace Dominion.Core.Dto.Interfaces
{
    /// <summary>
    /// The employee contact only has 2 phone numbers instead of 3 like the others.
    /// Due bad formatting of phone numbers in the database from legacy we need to normalize the format.
    /// </summary>
    public interface IEmployeePhoneNumbersDto
    {
        string HomePhoneNumber { get; set; }
        string CellPhoneNumber { get; set; }
    }
}