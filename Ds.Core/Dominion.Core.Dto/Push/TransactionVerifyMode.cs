namespace Dominion.Core.Dto.Push
{
    public enum TransactionVerifyMode : int
    {
        FingerOrBadgeOrPassword   = 128,
        Finger                    = 129,
        Pin                       = 130,
        Password                  = 131,
        Badge                     = 132,
        FingerOrPassword          = 133,
        FingerOrBadge             = 134,
        BadgeOrPassword           = 135,
        PinAndFinger              = 136,
        FingerAndPassword         = 137,
        BadgeAndFinger            = 138,
        BadgeAndPassword          = 139,
        BadgeAndPasswordAndFinger = 140,
        PinAndPasswordAndFinger   = 141,
        PinFingerOrBadgeFinger    = 142,
        PinOrBadge                = 143,
        Face                      = 148,
        FaceAndCard               = 149,
        FaceAndPin                = 150
    }
}
