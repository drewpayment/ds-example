using System;
using System.Linq.Expressions;

using Dominion.Utility.Containers;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Validation
{
    /// <summary>
    /// Wrapper for validation (verify) classes.
    /// Verify was used to reduce conflicting with FluentValidator's class/interface names.
    /// </summary>
    public interface IVerify<T>
        where T : class
    {
        /// <summary>
        /// Verifies the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <returns></returns>
        IOpResult Verify(T obj);

        /// <summary>
        /// Verifies only the specified properties for the object against pre-defined validation rules.
        /// </summary>
        /// <param name="obj">Object to verify.</param>
        /// <param name="properties">Properties to verify.</param>
        /// <returns></returns>
        IOpResult Verify(T obj, params Expression<Func<T, object>>[] properties);

        /// <summary>
        /// If populated, only the specified properties will be verified.
        /// </summary>
        PropertyList<T> PropertiesToValidate { get; set; }
    }
}
