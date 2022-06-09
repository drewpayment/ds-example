namespace Dominion.Core.Dto.Labor
{
    public interface IHasClockClientExceptionValidation
    {
        int ClockClientExceptionId { get; set; }
        int ClientId { get; set; }
        string Name { get; set; }
    }
}
