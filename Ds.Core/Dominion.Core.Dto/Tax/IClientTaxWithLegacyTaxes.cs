using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    // I wish c# supported intersection types with generics.
    // If it did, we could instead only have a single <T> where T : ILegacyTaxIdAndType
    // See: https://stackoverflow.com/a/45991173/13188284

    /// <summary>
    /// Interface defining ClientTax => LegacyTax relationships that are not possible to represent in EF due to schema limitations.
    /// </summary>
    /// <typeparam name="T">Some <see cref"ILegacyTaxIdAndType"/>, corresponding to some representation of <see cref="Dominion.Domain.Entities.Tax.Legacy.LegacyLocalTax"/>.</typeparam>
    /// <typeparam name="U">Some <see cref"ILegacyTaxIdAndType"/>, corresponding to some representation of <see cref="Dominion.Domain.Entities.Tax.Legacy.LegacyStateTax"/>.</typeparam>
    /// <typeparam name="V">Some <see cref"ILegacyTaxIdAndType"/>, corresponding to some representation of <see cref="Dominion.Domain.Entities.Tax.Legacy.LegacyDisabilityTax"/>.</typeparam>
    public interface IClientTaxWithLegacyTaxes<T, U, V>
        where T : ILegacyTaxIdAndType
        where U : ILegacyTaxIdAndType
        where V : ILegacyTaxIdAndType
    {
        int ClientTaxId { get; }
        LegacyTaxType LegacyTaxType { get; }

        T LegacyLocalTax { get; }
        T LegacyOtherTax { get; }
        U LegacyStateTax { get; }
        V LegacyDisabilityTax { get; }
    }

    public static class IClientTaxWithLegacyTaxesExtensions
    {
        public static bool IsStateTax<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
            where T : ILegacyTaxIdAndType
            where U : ILegacyTaxIdAndType
            where V : ILegacyTaxIdAndType
        {
            return self.LegacyTaxType == LegacyTaxType.StateWitholding
                    && self.LegacyStateTax != null;
        }

        public static bool IsSutaTax<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
            where T : ILegacyTaxIdAndType
            where U : ILegacyTaxIdAndType
            where V : ILegacyTaxIdAndType
        {
            return self.LegacyTaxType == LegacyTaxType.Suta
                    && self.LegacyOtherTax != null;
        }

        //public static bool IsFederalTax<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
        //    where T : ILegacyTaxIdAndType
        //    where U : ILegacyTaxIdAndType
        //    where V : ILegacyTaxIdAndType
        //{
        //    return self.LegacyTaxType == LegacyTaxType.FederalWitholding
        //            && self?.LegacyLocalTax == null
        //            && self?.LegacyOtherTax == null
        //            && self?.LegacyStateTax == null
        //            && self?.LegacyDisabilityTax == null;
        //}

        public static bool IsDisabilityTax<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
            where T : ILegacyTaxIdAndType
            where U : ILegacyTaxIdAndType
            where V : ILegacyTaxIdAndType
        {
            return (
                    self.LegacyTaxType == LegacyTaxType.Disability
                    || self.LegacyTaxType == LegacyTaxType.EmployerPaid
                ) && self.LegacyDisabilityTax != null;
        }

        public static bool IsLocalTax<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
            where T : ILegacyTaxIdAndType
            where U : ILegacyTaxIdAndType
            where V : ILegacyTaxIdAndType
        {
            return (
                    self.LegacyTaxType == LegacyTaxType.LocalResident
                    || self.LegacyTaxType == LegacyTaxType.LocalNonResident
                    || self.LegacyTaxType == LegacyTaxType.LocalSchool
                    || self.LegacyTaxType == LegacyTaxType.OtherTax
                ) && self.LegacyLocalTax != null;
        }

        /// <summary>
        /// See: <c>spGetEnabledTaxEntityFilingStatusesByClientTaxId</c>
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static ILegacyTaxIdAndType GetLegacyTaxIdAndType<T, U, V>(this IClientTaxWithLegacyTaxes<T, U, V> self)
            where T : ILegacyTaxIdAndType
            where U : ILegacyTaxIdAndType
            where V : ILegacyTaxIdAndType
        {
            ILegacyTaxIdAndType result;

            if (self.IsStateTax())
            {
                result = self.LegacyStateTax;
            }
            else if (self.IsSutaTax())
            {
                result = self.LegacyOtherTax;
            }
            else if (self.IsDisabilityTax())
            {
                result = self.LegacyDisabilityTax;
            }
            else if (self.IsLocalTax())
            {
                result = self.LegacyLocalTax;
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}
