namespace Dominion.Core.Dto.Labor
{
    public interface IHasTimePolicyRuleShiftSettings
    {
        decimal? MaxShift { get; }
        byte ApplyHoursOption { get; }
    }
}