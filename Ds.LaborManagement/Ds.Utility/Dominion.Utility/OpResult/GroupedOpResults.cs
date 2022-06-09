using System;
using System.Collections.Generic;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Container for holding the results of partitioning a <see cref="IEnumerable{IOpResult{TVal}}"/>,
    /// such that <see cref="IGroupedOpResults.ResultsWithoutErrors"/> 
    /// is the set complement to <see cref="IGroupedOpResults.ResultsWithErrors"/>.
    /// 
    /// See:
    /// <see cref="OpResultExtensions.PartitionResultsByPredicateForHasError"/>
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    public class GroupedOpResults<TVal> : IGroupedOpResults<TVal>
    {
        /// <summary>
        /// Values that may be considered good/valid to use for persistance or transmission.
        /// </summary>
        public IEnumerable<IOpResult<TVal>> ResultsWithoutErrors { get; set; }

        /// <summary>
        /// Values that were determined to have some error making then unsuitable for further use.
        /// Each element is left wrapped in an <see cref="IOpResult{TVal}"/> to allow for further proccessing,
        /// such as error reporting and logging.
        /// </summary>
        public IEnumerable<IOpResult<TVal>> ResultsWithErrors { get; set; }

        public GroupedOpResults(){}

        public GroupedOpResults(
            IEnumerable<IOpResult<TVal>> wrappedResults,
            Func<IOpResult<TVal>, bool> predicateForHasError
        )
        {
            var that = wrappedResults.PartitionResultsByPredicateForHasError(predicateForHasError);

            this.ResultsWithoutErrors = that.Data.ResultsWithoutErrors;
            this.ResultsWithErrors = that.Data.ResultsWithErrors;
        }
    }
}