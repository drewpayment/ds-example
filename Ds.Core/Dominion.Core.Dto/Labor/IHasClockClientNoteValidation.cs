namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientNoteValidation
    {
        int ClientId { get; set; }
        string Note { get; set; }
        bool IsHideFromEmployee { get; set; }
        bool IsActive { get; set; }
    }
}
