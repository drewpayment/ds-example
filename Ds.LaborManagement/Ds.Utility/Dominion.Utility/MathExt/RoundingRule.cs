using System;

namespace Dominion.Utility.MathExt
{
    public enum RoundingRule : byte
    {
        /// <summary>
        /// Also known as Banker's rounding. Rounds midpoint values to the nearest even significant digit (e.g. 0.5 => 1). 
        /// This is also the default method <see cref="System.Math.Round(decimal)"/> uses.
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.00
        /// 7.005 => 7.00
        /// 7.007 => 7.01
        /// 7.015 => 7.02
        /// 
        /// -7.005 => -7.00
        /// -7.015 => -7.02
        /// </example>
        RoundToEven = 1,

        /// <summary>
        /// The standard/traditional rounding method. Rounds midpoint values away from zero to nearst significant digit (e.g. 0.5 => 0).
        /// If value is positive, midpoint values will be rounded up.  If value is negative, midpoint value will be 
        /// rounded to the more negative value.
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.00
        /// 7.005 => 7.01
        /// 7.007 => 7.01
        /// 7.015 => 7.02
        /// 
        /// -7.005 => -7.01
        /// -7.015 => -7.02
        /// </example>
        RoundAwayFromZero = 2,

        /// <summary>
        /// Will always round down to the nearest significant digit towards zero (e.g. 0.9 => 0, -0.9 => 0).
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.00
        /// 7.005 => 7.00
        /// 7.007 => 7.00
        /// 7.015 => 7.01
        /// 
        /// -7.005 => -7.00
        /// -7.015 => -7.01
        /// </example>
        AlwaysRoundTowardsZero = 3,

        /// <summary>
        /// Will always round up to the significant digit away from zero (e.g. 0.1 => 1, -0.1 => -1).
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.01
        /// 7.005 => 7.01
        /// 7.007 => 7.01
        /// 7.015 => 7.02
        /// 
        /// -7.005 => -7.01
        /// -7.015 => -7.02
        /// </example>
        AlwaysRoundAwayFromZero = 4,

        /// <summary>
        /// Will always round up to the more positive significant digit (e.g. 0.1 => 1, -0.9 => 0). Similar to <see cref="Math.Ceiling"/>.
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.01
        /// 7.005 => 7.01
        /// 7.007 => 7.01
        /// 7.015 => 7.02
        /// 
        /// -7.005 => -7.00
        /// -7.015 => -7.01
        /// </example>
        AlwaysRoundUp = 5,

        /// <summary>
        /// Will always round down to the more negative significant digit (e.g. 0.9 => 0, -0.1 => -1). Similar to <see cref="Math.Floor"/>.
        /// </summary>
        /// <example>
        /// 2-Decimal Places:
        /// -----------------
        /// 7.001 => 7.00
        /// 7.005 => 7.00
        /// 7.007 => 7.00
        /// 7.015 => 7.01
        /// 
        /// -7.005 => -7.01
        /// -7.015 => -7.02
        /// </example>
        AlwaysRoundDown = 6
    }
}
