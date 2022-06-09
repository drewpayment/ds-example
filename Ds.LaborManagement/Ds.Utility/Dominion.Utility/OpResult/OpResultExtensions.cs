using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web.UI;
using Dominion.Utility.Constants;
using Dominion.Utility.DataExport.Exporters;
using Dominion.Utility.Messaging;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Extension methods for <see cref="IOpResult"/>s.
    /// </summary>
    public static class OpResultExtensions
    {
        /// <summary>
        /// Attempts to perform the given action within a try-catch. In the case of an exception, a 
        /// <see cref="BasicExceptionMsg"/> will be added to the result and the operation will be marked as failed.
        /// </summary>
        /// <param name="op">Operation result to append the action results to in case of failure.</param>
        /// <param name="action">Action to try to perform.</param>
        /// <returns>The resulting op result.</returns>
        public static TOpResult TryCatch<TOpResult>(this TOpResult op, Action action) where TOpResult : IOpResult
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                op.AddMessage(new BasicExceptionMsg(ex));
                op.SetToFail();
                IOpResultLogExceptionJaysMachine(ex);
            }
            return op;
        }

        public static async Task<TOpResult> TryCatchAsync<TOpResult>(this TOpResult op, Func<Task> task) where TOpResult : IOpResult
        {
            try
            {
                await task();
            }
            catch (Exception ex)
            {
                op.AddExceptionMessage(ex);
                op.SetToFail();
            }

            return op;
        }

        [Conditional(CommonConstants.BUILD_CONDITION_DEBUG)]
        public static void IOpResultLogExceptionJaysMachine(Exception ex)
        {
            var check = (Environment.MachineName == CommonConstants.JAYS_MACHINE_NAME_DEBUGGING);
            if(check)
            {
                var msg = new BasicExceptionMsg(ex);
                Json.I.SN(nameof(IOpResultLogExceptionJaysMachine)).TDTS().GenFile(msg);
            }

        }


        /// <summary>
        /// Attempts to perform the given action within a try-catch. In the case of an exception, a 
        /// <see cref="BasicExceptionMsg"/> will be added to the result and the operation will be marked as failed.
        /// </summary>
        /// <param name="op">Operation result to append the action results to in case of failure.</param>
        /// <param name="action">Action to try to perform.</param>
        /// <returns>The resulting op result.</returns>
        public static TOpResult TryCatch<TOpResult>(this TOpResult op, Action<TOpResult> action) where TOpResult : IOpResult
        {
            return op.TryCatch(() => action(op));
        }

        /// <summary>
        /// Performs the given data-getter action within the given op result's try-catch updating its success flag and 
        /// messages with any errors.  If successful, the data is returned. NOTE: No data is transfered to the original 
        /// <see cref="IOpResult"/>.
        /// </summary>
        /// <typeparam name="TData">Type of data being generated.</typeparam>
        /// <param name="op">The original op result whose Success-flag and messages will be updated based on the 
        /// success of getting the data.</param>
        /// <param name="dataGetter">Delegate used to get the data.</param>
        /// <returns>Op result containing the data (upon success) or error messages (upon failure).</returns>
        public static TData TryGetData<TData>(this IOpResult op, Func<TData> dataGetter)
        {
            var data = default(TData);

            op.TryCatch(() => data = dataGetter());

            return data;
        }

        /// <summary>
        /// Combines the success and messages of two <see cref="IOpResult"/> and returns the data of the result being 
        /// combined. This will NOT modify the data on the original <see cref="IOpResult"/>.
        /// </summary>
        /// <typeparam name="TData">Type of data to return.</typeparam>
        /// <param name="op">Original result success and messages will be merged into.</param>
        /// <param name="opWithData">Op result containing the success and messages to merge and the data to return.</param>
        /// <returns>Data from the result being merged.</returns>
        public static TData CombineAndReturnData<TData>(this IOpResult op, IOpResult<TData> opWithData)
        {
            op.CombineSuccessAndMessages(opWithData);
            return opWithData.Data;
        }

        /// <summary>
        /// Combines the success and message of two <see cref="IOpResult"/>s and returns the other result. This will 
        /// NOT modify the data of the original <see cref="IOpResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of result to return.</typeparam>
        /// <param name="op">Original result to merge success and messages into.</param>
        /// <param name="otherResult">Other result to merge success and messages from and then return.</param>
        /// <returns>Other op result.</returns>
        public static TResult CombineAndReturnOther<TResult>(this IOpResult op, TResult otherResult)
            where TResult : IOpResult
        {
            op.CombineSuccessAndMessages(otherResult);
            return otherResult;
        }

        /// <summary>
        /// Attempts to set the given <see cref="IOpResult{TData}.Data"/> with the result of provided data-getter.
        /// </summary>
        /// <typeparam name="TData">Type of data being generated.</typeparam>
        /// <param name="op">Op result to set the data on.</param>
        /// <param name="dataGetter">Delegate used to get the data.</param>
        /// <returns>Original op result with the data set (upon success) or error message (upon failure).</returns>
        public static IOpResult<TData> TrySetData<TData>(this IOpResult<TData> op, Func<TData> dataGetter)
        {
            op.TryCatch(() => op.Data = dataGetter());
            return op;
        }

        /// <summary>
        /// Checks if the specified parameter is null. If so, fails the operation and adds a 
        /// <see cref="NullReferenceMsg{TObj}"/> message.
        /// </summary>
        /// <typeparam name="TOpResult">Type of op result to validate with.</typeparam>
        /// <typeparam name="TObj">Type of object being checked for null.</typeparam>
        /// <param name="op">Op result to validate with.</param>
        /// <param name="obj">Object to check for null.</param>
        /// <param name="objectName">Friendly name of the object being checked.</param>
        /// <returns></returns>
        public static TOpResult CheckForNull<TOpResult, TObj>(this TOpResult op, TObj obj, string objectName = null) 
            where TOpResult : IOpResult
        {
            if (obj == null)
            {
                op.SetToFail().AddMessage(new NullReferenceMsg<TObj>(objectName));
            }

            return op;
        }

        /// <summary>
        /// Checks the specified object is null.  If so fails the operation and adds a <see cref="DataNotFoundMsg{TData}"/>
        /// message.
        /// </summary>
        /// <typeparam name="TOpResult"></typeparam>
        /// <typeparam name="TObj"></typeparam>
        /// <param name="op">Op result to validate with.</param>
        /// <param name="obj">Object to check for null.</param>
        /// <returns></returns>
        public static TOpResult CheckForNotFound<TOpResult, TObj>(this TOpResult op, TObj obj) 
            where TOpResult : IOpResult
            where TObj : class
        {
            if (obj == null)
            {
                op.SetToFail().AddMessage(new DataNotFoundMsg<TObj>(MsgLevels.Error));
            }

            return op;
        }

        /// <summary>
        /// highfix: jay: needs unit test.
        /// Only if the result is currently successful then ...
        /// Attempts to perform the given action within a try-catch. In the case of an exception, a 
        /// <see cref="BasicExceptionMsg"/> will be added to the result and the operation will be marked as failed.
        /// </summary>
        /// <param name="op">Operation result to append the action results to in case of failure.</param>
        /// <param name="action">Action to try to perform.</param>
        /// <returns>The resulting op result.</returns>
        public static TResult TryCatchIfSuccessful<TResult>(this TResult op, Action action) where TResult : IOpResult
        {
            if(op.Success)
                op.TryCatch(action);

            return op;
        }

        /// <summary>
        /// Only attempt to perform the action if the original was successful. Action will be passed the original 
        /// op result.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="op">Operation to catch any errors on.</param>
        /// <param name="action">Action to perform using the orignal op result.</param>
        /// <returns></returns>
        public static TResult TryCatchIfSuccessful<TResult>(this TResult op, Action<TResult> action) where TResult : IOpResult
        {
            if(op.Success)
                op.TryCatch(action);

            return op;
        }

        /// <summary>
        /// Merges the results of the current <see cref="IOpResult"/> into another <see cref="IOpResult"/>.  Returns 
        /// the original <see cref="IOpResult"/> to be further managed.
        /// </summary>
        /// <param name="source">Op result containing the status and messages to merge into the destination result.</param>
        /// <param name="dest">Op result to merge results into.</param>
        public static TSourceResult MergeInto<TSourceResult, TDestResult>(this TSourceResult source, TDestResult dest) 
            where TSourceResult : IOpResult
            where TDestResult : IOpResult
        {
            if(dest != null)
                dest.CombineSuccessAndMessages(source);

            return source;
        }

        /// <summary>
        /// Merges the current result into the destination result by performing a <see cref="IOpResult{T}.CombineAll"/>. 
        /// If the result status is successful the data on the current result will be transfered to the destination
        /// result.
        /// </summary>
        /// <typeparam name="TData">Type of data being merged.</typeparam>
        /// <param name="source">Source result containing the status, messages and data to merge into the destination.</param>
        /// <param name="dest">Destination result all status, messages and data will be merged into.</param>
        /// <param name="requireSuccessToMergeData">If true (default), data will only be merged if the resulting operation is successful.</param>
        /// <returns></returns>
        public static IOpResult<TData> MergeAll<TData>(this IOpResult<TData> source, IOpResult<TData> dest, bool requireSuccessToMergeData = true) 
        {
            if(dest != null)
                dest.CombineAll(source, requireSuccessToMergeData);

            return source;
        }

        public static IOpResult<IEnumerable<TData>> MergeAllCombineData<TData>(
            this IOpResult<IEnumerable<TData>> source, 
            IOpResult<IEnumerable<TData>> dest)
        {
            dest.Data= dest.Data.ToOrNewList();
            ((List<TData>) dest.Data)?.AddRange(source.Data);

            return dest.MergeInto(dest);
        }

        public static IOpResult<IEnumerable<TData>> PerformOnMulitpleClients<TData>(
            this IOpResult<IEnumerable<TData>> source,
            IEnumerable<int> clientIds,
            Func<int, IOpResult<IEnumerable<TData>>> queryDelegate)
        {
            var opResult = new OpResult<IEnumerable<TData>>() { Data = new List<TData>() };

            clientIds.ForEach(ci =>
            {
                var result = queryDelegate(ci);
                result.MergeAllCombineData(opResult);
            });

            opResult.MergeAllCombineData(source);


            return source;
        }

        /// <summary>
        /// Sets the data of the result if the operation was successful up to this point.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <param name="result">Result to set data on.</param>
        /// <param name="data">Data to set.</param>
        /// <returns></returns>
        public static IOpResult<TData> SetDataOnSuccess<TData>(this IOpResult<TData> result, TData data)
        {
            if(result.Success)
                result.Data = data;

            return result;
        }

        /// <summary>
        /// A "simple" way to chain extra code on the result of an OpResult. Handles extracting data, exceptions,
        /// edge cases around failure and exceptions, short-circuit failure, and is chainable.
        /// 
        /// This version expects the func-argument to also return an OpResult. So, use this when you have a pure-argument
        /// function that can sometimes fail in an OpResult-described way.
        /// 
        /// See the unit tests for more precise descriptions.
        /// </summary>
        /// <typeparam name="TArg">The type of the data coming in from up the chain.</typeparam>
        /// <typeparam name="TReturn">Return type for data down the chain.</typeparam>
        /// <param name="argument">An OpResult containing data you want to process further.</param>
        /// <param name="f">A function/lambda/MethodGroup representing the further work to be done.</param>
        /// <returns>An OpResult describing the overall result of the chain.</returns>
        public static IOpResult<TReturn> Then<TArg, TReturn>(this IOpResult<TArg> argument, Func<TArg, IOpResult<TReturn>> f)
        {
            if (argument.Success)
            {
                try
                {
                    return f(argument.Data);
                }
                catch (ApplicationException ae)
                {
                    var exceptionOpResult = new OpResult<TReturn>();
                    argument.MergeInto(exceptionOpResult);
                    exceptionOpResult.SetToFail();
                    exceptionOpResult.AddExceptionMessage(ae);
                    return exceptionOpResult;
                }
            }
            var failedResult = new OpResult<TReturn>();
            argument.MergeInto(failedResult);
            return failedResult;
        }

        /// <summary>
        /// A "simple" way to chain extra code on the result of an OpResult. Handles extracting data, exceptions,
        /// edge cases around failure and exceptions, short-circuit failure, and is chainable.
        /// 
        /// This version expects the func-argument to reuturn a pure value or throw an exception, and wraps the results
        /// of running the func in an OpResult, so the chain can continue.
        /// 
        /// See the unit tests for more precise descriptions.
        /// </summary>
        /// <typeparam name="TArg">The type of the data coming in from up the chain.</typeparam>
        /// <typeparam name="TReturn">Return type for data down the chain.</typeparam>
        /// <param name="argument">An OpResult containing data you want to process further.</param>
        /// <param name="f">A function/lambda/MethodGroup representing the further work to be done.</param>
        /// <returns>An OpResult describing the overall result of the chain.</returns>
        public static IOpResult<TReturn> Then<TArg, TReturn>(this IOpResult<TArg> argument, Func<TArg, TReturn> f)
        {
            var returnedOpResult = new OpResult<TReturn>();
            argument.MergeInto(returnedOpResult);
            if (argument.Success)
            {
                returnedOpResult.TrySetData(() => f(argument.Data));
                //returnedOpResult.MergeInto(argument);
            }
            return returnedOpResult;
        }

        /// <summary>
        /// Sometimes with save-multiple and other cases where you have a list of stuff to do DB work with,
        /// you'll bump into type signatures like that of `self`. Select takes IOpResult-IEnumerable-IOpResult,
        /// and flattens it out, while applying a function to each item.
        /// 
        /// Handles exceptions and OpResult failures. One failure fails the overall operation. Preserves element
        /// count in the face of failure via <code>default(TReturn)</code>.
        /// </summary>
        /// <typeparam name="TArg">Type the IEnumerable ultimately contains.</typeparam>
        /// <typeparam name="TReturn">Return type of the function, as contained in the IEnumerable.</typeparam>
        /// <param name="self">The "this". The messy enumerable you want to run a function on.</param>
        /// <param name="func">The function to apply to each item in the IEnumerable. Does not need to know about OpResult.</param>
        /// <returns>A single OpResult with the list of results in it. <code>func</code> will have been called on each argument, and 
        /// any exceptions cause a default value to be added instead.</returns>
        public static IOpResult<IEnumerable<TReturn>> Select<TArg, TReturn>(
            this IOpResult<IEnumerable<IOpResult<TArg>>> self, Func<TArg, TReturn> func)
        {
            var returnedOpResult = new OpResult<IEnumerable<TReturn>>();
            if (!self.Success)
            {
                self.MergeInto(returnedOpResult);
                return returnedOpResult;
            }
            returnedOpResult.CombineMessages(self);
            var ienum = self.Data;
            var returnedData = new List<TReturn>();
            foreach (var opResult in ienum)
            {
                if (!opResult.Success || !returnedOpResult.Success)
                {
                    returnedOpResult.SetToFail();
                }
                returnedOpResult.CombineMessages(opResult);
                if (opResult.Success)
                {
                    try
                    {
                        returnedData.Add(func(opResult.Data));
                    }
                    catch (Exception e)
                    {
                        returnedData.Add(default(TReturn));
                        returnedOpResult.AddExceptionMessage(e);
                        returnedOpResult.SetToFail();
                    }
                }
                else
                {
                    returnedData.Add(default(TReturn));
                    // Messages and success/failure status handled above.
                }
            }
            returnedOpResult.Data = returnedData;
            return returnedOpResult;
        }

        /// <summary>
        /// Sometimes with save-multiple and other cases where you have a list of stuff to do DB work with,
        /// you'll bump into type signatures like that of `self`. Select takes IOpResult-IEnumerable-IOpResult,
        /// and flattens it out, while applying a function to each item.
        /// 
        /// Handles exceptions and OpResult failures. One failure fails the overall operation. Preserves element
        /// count in the face of failure via <code>default(TReturn)</code>.
        /// </summary>
        /// <typeparam name="TArg">Type the IEnumerable ultimately contains.</typeparam>
        /// <typeparam name="TReturn">Return type of the function, as contained in the IEnumerable.</typeparam>
        /// <param name="self">The "this". The messy enumerable you want to run a function on.</param>
        /// <param name="func">The function to apply to each item in the IEnumerable. Does not need to know about OpResult.</param>
        /// <returns>A single OpResult with the list of results in it. <code>func</code> will have been called on each argument, and 
        /// any exceptions cause a default value to be added instead.</returns>
        public static IOpResult<IEnumerable<TReturn>> Select<TArg, TReturn>(
            this IOpResult<IEnumerable<IOpResult<TArg>>> self, Func<TArg, IOpResult<TReturn>> func)
        {
            var returnedOpResult = new OpResult<IEnumerable<TReturn>>();
            if (!self.Success)
            {
                self.MergeInto(returnedOpResult);
                return returnedOpResult;
            }
            returnedOpResult.CombineMessages(self);
            var ienum = self.Data;
            var returnedData = new List<TReturn>();
            foreach (var opResult in ienum)
            {
                if (!opResult.Success || !returnedOpResult.Success)
                {
                    returnedOpResult.SetToFail();
                }
                returnedOpResult.CombineMessages(opResult);
                if (opResult.Success)
                {
                    try
                    {
                        var newOpResult = func(opResult.Data);
                        if (!newOpResult.Success)
                        {
                            returnedOpResult.SetToFail();
                        }
                        returnedOpResult.CombineMessages(newOpResult);
                        returnedData.Add(newOpResult.Data);
                    }
                    catch (Exception e)
                    {
                        returnedData.Add(default(TReturn));
                        returnedOpResult.AddExceptionMessage(e);
                        returnedOpResult.SetToFail();
                    }
                }
                else
                {
                    returnedData.Add(default(TReturn));
                    // Messages and success/failure status handled above.
                }
            }
            returnedOpResult.Data = returnedData;
            return returnedOpResult;
        }

        /// <summary>
        /// Convert the messages into validation status messages.
        /// </summary>
        /// <param name="r"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public static IEnumerable<ValidationStatusMessage> ToValidationStatusMsgs(this IOpResult r, ValidationStatusMessageType statusType = ValidationStatusMessageType.General)
        {
            var list = new List<ValidationStatusMessage>();

            foreach (var msg in r.MsgObjects)
            {
                var vsm = msg.ToValidationStatusMsg(statusType);
                list.Add(vsm);
            }

            return list;
        }

        /// <summary>
        /// Convert the messages into validation status messages.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="statusType"></param>
        /// <returns></returns>
        public static ValidationStatusMessage ToValidationStatusMsg(this IMsgSimple msg, ValidationStatusMessageType statusType = ValidationStatusMessageType.General)
        {
            var hasExeception = msg is BasicExceptionMsg;

            var vsm = new ValidationStatusMessage(
                text: msg.Msg,
                messageType: statusType,
                memberNames: null,
                level: (StatusMessageLevelType)((int)msg.Level),
                sourceException: hasExeception ? ((BasicExceptionMsg)msg).Exception : null
            );

            return vsm;
        }

        public static void FailWhenFalse(this IOpResult result, bool criteria, IMsgSimple msg = null)
        {
            if (!criteria)
            {
                result.SetToFail();

                if (msg != null)
                    result.AddMessage(msg);
            }
        }

        public static void FailWhenTrue(this IOpResult result, bool criteria, IMsgSimple msg = null)
        {
            if (criteria)
            {
                result.SetToFail();

                if (msg != null)
                    result.AddMessage(msg);
            }
        }

        /// <summary>
        /// Allows us to fail the <see cref="IOpResult"/>, pass a message and continue 
        /// chaining.
        /// </summary>
        /// <param name="msgBuilder"></param>
        /// <returns></returns>
        public static TOpResult SetToFail<TOpResult>(this TOpResult result, Func<IMsgSimple> msgBuilder = null) where TOpResult : IOpResult
        {
            result.Success = false;
            if (msgBuilder != null)
            {
                result.AddMessage(msgBuilder());
            }
            return result;
        }

        //public static IOpResult AddMessagesToValidationObject<T>(this IOpResult r, IValidationObject vo)
        //{
        //    //ValidationStatusMessage
        //    foreach (var msg in r.MsgObjects)
        //    {
        //        var exMsg = msg as BasicExceptionMsg;

        //        if (exMsg != null)
        //        {
        //            vo.ValidationMessages.Add(
        //                text: exMsg.Msg,
        //                fieldNames: null,
        //                level: StatusMessageLevelType.Error,
        //                messageType: ValidationStatusMessageType.General,
        //                sourceException: exMsg.Exception);

        //            r.SetToFail();
        //        }
        //    }
        //    return r;
        //}
        public static TOpResult SetToFail<TOpResult>(this TOpResult result, string failureMessage) where TOpResult : IOpResult
        {
            result.Success = false;
            if (!string.IsNullOrEmpty(failureMessage))
            {
                result.AddMessage(new GenericMsg(failureMessage));
            }
            return result;
        }

        /// <summary>
        /// Partitions <paramref name="wrappedResults"/> into two groups, 
        /// <see cref="IGroupedOpResults.ResultsWithoutErrors"/> 
        /// and <see cref="IGroupedOpResults.ResultsWithErrors"/> 
        /// according to some boolean predicate function, <paramref name="predicateForHasError"/>.
        /// 
        /// The <paramref name="predicateForHasError"/> determines which of the two groups a given element are placed into, where
        /// any element where the predicate returns <c>true</c> is considered to be an element with an error,
        /// and is placed into the returned <see cref="IGroupedOpResults.ResultsWithErrors"/>.
        /// 
        /// Any element where the predicate returns <c>false</c> is considered to be "good", 
        /// and is placed into the returned <see cref="IGroupedOpResults.ResultsWithoutErrors"/>.
        /// <typeparam name="TSource"></typeparam>
        /// <param name="wrappedResults">
        /// An enumerable where each element has its own distinct success/fail state and error messages.
        /// </param>
        /// <param name="predicateForHasError">
        /// If unspecified, defaults to <see cref="IOpResult.HasError"/>.
        /// Else if specified, uses the specified function as the ONLY partitioning predicate.
        /// (no additional default predicate criteria used if specified as non-null).
        /// </param>
        /// <returns>
        /// Dto with named properties for each of the two partitions.
        /// </returns>
        /// <remarks>See: <see cref="GroupedOpResults{TSource}"/></remarks>
        public static IOpResult<IGroupedOpResults<TSource>> 
        PartitionResultsByPredicateForHasError<TSource>(
            this IEnumerable<IOpResult<TSource>> wrappedResults,
            Func<IOpResult<TSource>, bool> predicateForHasError
        )
        {
            var result = new OpResult<IGroupedOpResults<TSource>>();

            result.TrySetData(() => {
                var resultData = new GroupedOpResults<TSource>();

                var partitionedByPredicate = (predicateForHasError is null)
                    ? wrappedResults.GroupBy(x => x.HasError, x => x)
                    : wrappedResults.GroupBy(predicateForHasError, x => x)
                    ;

                resultData.ResultsWithErrors = partitionedByPredicate
                    .Where(hasError => hasError.Key)
                    .SelectMany(grouping => grouping);

                resultData.ResultsWithoutErrors = partitionedByPredicate
                    .Where(hasError => !hasError.Key)
                    .SelectMany(grouping => grouping);

                return resultData;
            });

            return result;
        }
    }
}
