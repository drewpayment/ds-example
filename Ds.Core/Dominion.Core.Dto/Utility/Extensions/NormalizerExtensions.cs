using Dominion.Core.Dto.Interfaces;
using Dominion.Utility.Transform;

namespace Dominion.Core.Dto.Utility.Extensions
{
    public static class NormalizerExtensions
    {
        /// <summary>
        /// Normalize a users contact name.
        /// </summary>
        /// <param name="obj">An object that implements the interface.</param>
        public static void NormalizeContactName(this IContactNameDto obj)
        {
            if (obj.FirstName != null)
                obj.FirstName = obj.FirstName.Trim();

            if (obj.MiddleInitial != null)
                obj.MiddleInitial = obj.MiddleInitial.Trim();

            if (obj.LastName != null)
                obj.LastName = obj.LastName.Trim();
        }

        /// <summary>
        /// Normalizes phone numbers.
        /// </summary>
        /// <param name="obj">An object that implements the specified interface.</param>
        public static void NormalizePhoneNumbers(this IPhoneNumbersDto obj)
        {
            NormalizePhoneNumberTypes(obj);
        }

        /// <summary>
        /// Normalizes phone numbers.
        /// </summary>
        /// <param name="obj">An object that implements the specified interface.</param>
        public static void NormalizePhoneNumbers(this IEmployeePhoneNumbersDto obj)
        {
            NormalizePhoneNumberTypes(obj);
        }

        /// <summary>
        /// Handle the normalization of the phone number types.
        /// </summary>
        /// <param name="obj"></param>
        private static void NormalizePhoneNumberTypes(dynamic obj)
        {
            var transformer = new PhoneNumberTenNumbersWithDashes();
            obj.HomePhoneNumber = transformer.Transform(obj.HomePhoneNumber);
            obj.CellPhoneNumber = transformer.Transform(obj.CellPhoneNumber);
        }
    }
}