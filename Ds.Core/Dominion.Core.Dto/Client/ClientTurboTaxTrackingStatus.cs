namespace Dominion.Core.Dto.Client
{
    public enum ClientTurboTaxTrackingStatus : byte
    {
        Processing = 0,
        Completed  = 1,
        Incomplete = 2,
        Failed     = 3,
        NotFound   = 4
    }
}
