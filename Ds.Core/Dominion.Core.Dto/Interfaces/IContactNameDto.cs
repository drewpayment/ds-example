namespace Dominion.Core.Dto.Interfaces
{
    /// <summary>
    /// Represents the parts of a contacts name.
    /// </summary>
    public interface IContactNameDto
    {
        string FirstName { get; set; }
        string MiddleInitial { get; set; }
        string LastName { get; set; }
    }
}