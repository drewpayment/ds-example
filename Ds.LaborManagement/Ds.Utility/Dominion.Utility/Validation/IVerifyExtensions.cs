using System;
using System.Linq.Expressions;

using Dominion.Utility.Containers;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Validation
{
    public static class IVerifyExtensions
    {
        /// <summary>
        /// Adds the indicated properties to the <see cref="IVerify{T}.PropertiesToValidate"/>.
        /// </summary>
        /// <typeparam name="T">Verifier type.</typeparam>
        /// <param name="verifier">Verifier to update properties to validate.</param>
        /// <param name="properties">Property expressions to add to the verifier.</param>
        /// <returns></returns>
        public static IVerify<T> SelectPropertiesToValidate<T>(
            this IVerify<T> verifier, 
            params Expression<Func<T, object>>[] properties) where T : class
        {
            if (properties != null)
            {
                if (verifier.PropertiesToValidate == null) 
                    verifier.PropertiesToValidate = new PropertyList<T>();

                properties.ForEach(p => verifier.PropertiesToValidate.Include(p));
            }

            return verifier;
        }
    }
}
