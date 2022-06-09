using System;
using Dominion.Utility.Msg;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Get information about the result of executing an operation (function).
    /// Also return an object that may or may not be needed based on the status of the operation.
    /// ie. If successful, the object will be used. If unsuccessful the object may still be needed or be null.
    /// </summary>
    /// <typeparam name="TVal">The return type.</typeparam>
    public class OpResult<TVal> : OpResultBase<OpResult<TVal>>, IOpResult<TVal>
    {
        /// <summary>
        /// The object being returned outside of the operation messages or success stats.
        /// </summary>
        public TVal Data { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">A value for the data this op result contains.</param>
        /// <param name="createFrom">If you have an existing IOpResult you can create this object from those details.</param>
        public OpResult(TVal value = default(TVal), IOpResult createFrom = null)
        {
            Data = value;
            CombineSuccessAndMessages(createFrom);
        }

        /// <summary>
        /// True if the data object isn't null (alias of 'IsDataNotNull').
        /// </summary>
        public bool HasData
        {
            get { return IsDataNotNull; }
        }

        /// <summary>
        /// True if the data object isn't null.
        /// </summary>
        public bool IsDataNotNull
        {
            get { return !IsDataNull; }
        }

        /// <summary>
        /// True if the data object is null.
        /// </summary>
        public bool IsDataNull
        {
            get { return Data == null; }
        }

        /// <summary>
        /// True if the data object is its type's default value.
        /// </summary>
        public bool IsDataDefaultValue
        {
            get { return IsDataNull || Data.Equals(default(TVal)); }
        }

        /// <summary>
        /// Has no error and the data object isn't null.
        /// </summary>
        public bool HasNoErrorAndHasData
        {
            get { return HasNoError && IsDataNotNull; }
        }

        /// <summary>
        /// Combine success, messages and data (if exists) from other result.
        /// note: jay: I'm thinking CombineAll should replace CombineSuccessAndMessages but I'm not doing that right now. 
        /// note: jay: It may be down the road 'All' doesn't fit.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <param name="requireSuccessToCombineData">If true, will only combine data if the <see cref="otherResult"/> has succeeded.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult CombineAll(IOpResult otherResult, bool requireSuccessToCombineData = true)
        {
            if(otherResult is IOpResult<TVal> && (!requireSuccessToCombineData || otherResult.Success))
            {
                var result = otherResult as IOpResult<TVal>;
                Data = result.Data;
            }

            return CombineSuccessAndMessages(otherResult);
        }

        /// <summary>
        /// Fails the <see cref="IOpResult"/> if it currently does not contain data.
        /// </summary>
        /// <param name="msgBuilder">Delegate used to construct an error message when no data is found.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult<TVal> CheckForData(Func<IMsgSimple> msgBuilder = null)
        {
            if (!HasData)
            {
                this.SetToFail();
                if (msgBuilder != null)
                {
                    this.AddMessage(msgBuilder());
                }
            }

            return this;
        }

        /// <summary>
        /// Used to help the based class return the derived type in a chaining.
        /// </summary>
        /// <returns></returns>
        protected override OpResult<TVal> ChainReturn()
        {
            return this;
        }
    }
}