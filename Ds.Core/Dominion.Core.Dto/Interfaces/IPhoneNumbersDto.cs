namespace Dominion.Core.Dto.Interfaces
{
    /// <summary>
    /// This is going to hopefully be considered the norm for phone numbers.
    /// That means if it's a contact it will contain all the phone numbers mentioned in this interface.
    /// Due bad formatting of phone numbers in the database from legacy we need to normalize the format.
    /// </summary>
    public interface IPhoneNumbersDto
    {
        string HomePhoneNumber { get; set; }
        string CellPhoneNumber { get; set; }
    }
}