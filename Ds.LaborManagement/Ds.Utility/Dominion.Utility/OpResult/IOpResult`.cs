using System;

using Dominion.Utility.Msg;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Defines an op result that returns a value.
    /// </summary>
    /// <typeparam name="TDataType"></typeparam>
    public interface IOpResult<TDataType> : IOpResult
    {
        /// <summary>
        /// The object being returned outside of the operation messages or success stats.
        /// </summary>
        TDataType Data { get; set; }

        /// <summary>
        /// True if the data object isn't null.
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// True if the data object isn't null.
        /// </summary>
        bool IsDataNotNull { get; }

        /// <summary>
        /// True if the data object is null.
        /// </summary>
        bool IsDataNull { get; }

        /// <summary>
        /// True if the data object is its type's default value.
        /// </summary>
        bool IsDataDefaultValue { get; }

        /// <summary>
        /// Has no error and the data object isn't null.
        /// </summary>
        bool HasNoErrorAndHasData { get; }

        /// <summary>
        /// Combine success, messages and data (if exists) from other result.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <param name="requireSuccessToCombineData">If true, will only combine data if the <see cref="otherResult"/> has succeeded.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult CombineAll(IOpResult otherResult, bool requireSuccessToCombineData = true);

        /// <summary>
        /// Fails the <see cref="IOpResult"/> if it currently does not contain data.
        /// </summary>
        /// <param name="msgBuilder">Delegate used to construct an error message when no data is found.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult<TDataType> CheckForData(Func<IMsgSimple> msgBuilder = null); 
    }
}