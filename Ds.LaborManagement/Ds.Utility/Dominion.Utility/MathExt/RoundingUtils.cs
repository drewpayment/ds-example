using System;

namespace Dominion.Utility.MathExt
{
    /// <summary>
    /// Rounding utilities used to extend the built-in <see cref="Dominion.Utility.Math.Round"/> methods.
    /// </summary>
    /// <remarks>
    /// Modified From: http://stackoverflow.com/questions/13482159/how-to-round-up-or-down-in-c
    /// </remarks>
    public static class RoundingUtils
    {
        /// <summary>
        /// Rounds a decimal value to the specified number of significant digits using the provided rounding rule.
        /// </summary>
        /// <param name="value">Value to round.</param>
        /// <param name="roundTo">Number to round to (e.g. 5 will round to the nearest multiple of 5)</param>
        /// <param name="roundingRule">Rounding rule to apply when rounding. Default is <see cref="RoundingRule.RoundAwayFromZero"/>.</param>
        /// <returns></returns>
        public static decimal RoundToValue(decimal value, decimal roundTo, RoundingRule roundingRule = RoundingRule.RoundAwayFromZero)
        {
            roundTo = Math.Abs(roundTo);

            //from:http://stackoverflow.com/a/1531724
            return (int)Round(value / roundTo, decimals:0, roundingRule:roundingRule) * roundTo;
        }


        /// <summary>
        /// Rounds a decimal value to the specified number of significant digits using the provided rounding rule.
        /// </summary>
        /// <param name="value">Value to round.</param>
        /// <param name="decimals">Number of significant digits to round to. Default is 0.</param>
        /// <param name="roundingRule">Rounding rule to apply when rounding. Default is <see cref="RoundingRule.RoundAwayFromZero"/>.</param>
        /// <returns></returns>
        public static decimal Round(decimal value, int decimals = 0, RoundingRule roundingRule = RoundingRule.RoundAwayFromZero)
        {
            switch (roundingRule)
            {
                case RoundingRule.RoundToEven:
                    return System.Math.Round(value, decimals, MidpointRounding.ToEven);
                case RoundingRule.RoundAwayFromZero:
                    return System.Math.Round(value, decimals, MidpointRounding.AwayFromZero);
                case RoundingRule.AlwaysRoundTowardsZero:
                    return value > 0 ? RoundDown(value, decimals) : RoundUp(value, decimals);
                case RoundingRule.AlwaysRoundAwayFromZero:
                    return value > 0 ? RoundUp(value, decimals) : RoundDown(value, decimals);
                case RoundingRule.AlwaysRoundDown:
                    return RoundDown(value, decimals);
                case RoundingRule.AlwaysRoundUp:
                    return RoundUp(value, decimals);
                default:
                    throw new ArgumentOutOfRangeException(nameof(roundingRule), roundingRule, null);
            }
        }

        /// <summary>
        /// Rounds the specified value up to the specified number of decimal-places. Equivalent of performing
        /// <see cref="System.Math.Ceiling"/> to a specified number of decimals.
        /// </summary>
        /// <param name="value">Value to round up.</param>
        /// <param name="places">Number of significant digits to round up to.</param>
        /// <returns></returns>
        public static decimal RoundUp(decimal value, int places)
        {
            if(places == 0)
                return System.Math.Ceiling(value);

            var factor = RoundFactor(places);
            value *= factor;
            value = System.Math.Ceiling(value);
            value /= factor;
            return value;
        }

        /// <summary>
        /// Rounds the specified value down to the specified number of decimal-places. Equivalent of performing
        /// <see cref="Dominion.Utility.Math.Floor"/> to a specified number of decimals.
        /// </summary>
        /// <param name="value">Value to round down.</param>
        /// <param name="places">Number of significant digits to round down to.</param>
        /// <returns></returns>
        public static decimal RoundDown(decimal value, int places)
        {
            if(places == 0)
                return System.Math.Floor(value);

            var factor = RoundFactor(places);
            value *= factor;
            value = System.Math.Floor(value);
            value /= factor;
            return value;
        }
        
        /// <summary>
        /// Returns a multiplier representing a specified number of decimal-places.
        /// </summary>
        /// <param name="places">Number of decimal places being rounded to.</param>
        /// <returns></returns>
        internal static decimal RoundFactor(int places)
        {
            var factor = 1m;

            if (places < 0)
            {
                places = -places;
                for (var i = 0; i < places; i++)
                    factor /= 10m;
            }
            else
            {
                for (var i = 0; i < places; i++)
                    factor *= 10m;
            }

            return factor;
        }
    }
}