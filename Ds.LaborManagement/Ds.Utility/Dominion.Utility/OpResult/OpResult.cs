using Dominion.Utility.Logging;

namespace Dominion.Utility.OpResult
{

    public static class OpResultConstants
    {
        public static IDsLogger DefaultLogger { get; set; } = new DsLogger();
    }
    /// <summary>
    /// A class for all operations that only need to signal success.
    /// </summary>
    public class OpResult : OpResultBase<OpResult>
    {
        protected override OpResult ChainReturn()
        {
            return this;
        }

        /// <summary>
        /// Constructs a new <see cref="OpResult{TVal}"/> from the given result object.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="result">The value that the new result should be created with.</param>
        /// <returns></returns>
        public static IOpResult<TResult> From<TResult>(TResult result)
        {
            return new OpResult<TResult>(result);
        }
    }
}