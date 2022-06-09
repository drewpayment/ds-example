using System;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Validation;

namespace Dominion.Utility.OpTasks
{
    /// <summary>
    /// Used to handle both mapping and validation. Chainable.
    /// </summary>
    /// <typeparam name="TSource">Usually a DTO but not necessary.</typeparam>
    /// <typeparam name="TDest">Usually an Entity but not necessary.</typeparam>
    public interface IVerifyMap<TSource, TDest> 
        where TSource : class 
        where TDest : class
    {
        /// <summary>
        /// This should via the setup method.
        /// If null is passed in it will be set to a new instance
        /// </summary>
        IOpResult OpResult { get; }

        /// <summary>
        /// Represents the source object of the mapping scenario.
        /// Usually a DTO but not necessary.
        /// </summary>
        TSource Source { get; set; }

        /// <summary>
        /// Represents the destination object of the mapping scenario.
        /// Usually an entity but not necessary.
        /// </summary>
        TDest Dest { get; set; }

        /// <summary>
        /// Map using the internal factory to resolve the mapper.
        /// </summary>
        /// <returns></returns>
        IVerifyMap<TSource, TDest> Map();

        /// <summary>
        /// Map using a delegate Func.
        /// </summary>
        /// <returns></returns>
        IVerifyMap<TSource, TDest> Map(Func<TSource, TDest> mapFunc, bool requireSuccess = true);

        /// <summary>
        /// Verify using the internal factory to resolve the verifier.
        /// </summary>
        /// <returns></returns>
        IVerifyMap<TSource, TDest> ValidateDest();

        ///// <summary>
        ///// Set the source and the result object on the object.
        ///// </summary>
        ///// <param name="source">The source dto.</param>
        ///// <param name="opResult">The result from the current service using this task.</param>
        ///// <returns></returns>
        //IVerifyMap<TSource, TDest> Setup(TSource source, IOpResult opResult);

    }
}