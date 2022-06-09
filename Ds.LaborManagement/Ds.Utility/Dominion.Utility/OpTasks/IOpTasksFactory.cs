using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Validation;

namespace Dominion.Utility.OpTasks
{
    public interface IOpTasksFactory
    {
        /// <summary>
        /// Get a mapper object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="TSource">The source object.</typeparam>
        /// <typeparam name="TDest">The destination object.</typeparam>
        /// <returns></returns>
        IMapper<TSource, TDest> GetMapper<TSource, TDest>();

        /// <summary>
        /// Get a verifier (object validation ie. fluent validation) object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="T">The object to validate.</typeparam>
        /// <returns></returns>
        IVerify<T> GetVerifier<T>()
            where T : class;

        /// <summary>
        /// Get a verifier (object validation ie. fluent validation) object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TVerify">Type of verifier to get.</typeparam>
        /// <returns></returns>
        TVerify GetVerifier<T, TVerify>()
            where T : class
            where TVerify : IVerify<T>;
        
        /// <summary>
        /// Register a mapper object.
        /// </summary>
        /// <typeparam name="TSource">The source object.</typeparam>
        /// <typeparam name="TDest">The destination object.</typeparam>
        /// <param name="mapper"></param>
        IOpTasksFactory RegisterMapper<TSource, TDest>(IMapper<TSource, TDest> mapper);

        /// <summary>
        /// Register a validator.
        /// </summary>
        /// <typeparam name="T">The object being validated.</typeparam>
        /// <param name="verifier">The validator object.</param>
        IOpTasksFactory RegisterValidator<T>(IVerify<T> verifier)
            where T : class;

        /// <summary>
        /// Creates an instance of IVeryifyMap.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDest">The destination type.</typeparam>
        /// <returns></returns>
        IVerifyMap<TSource, TDest> BuildVerifyMap<TSource, TDest>(TSource obj, IOpResult result) 
            where TSource : class
            where TDest : class;

        void ExecuteAction<T>(
            IAdHocAction<T> action, 
            T obj);

        void ExecuteAction<T1, T2>(
            IAdHocAction<T1, T2> action, 
            T1 obj1,
            T2 obj2);

        void ExecuteAction<T1, T2, T3>(
            IAdHocAction<T1, T2, T3> action, 
            T1 obj1,
            T2 obj2,
            T3 obj3);

        void ExecuteAction<T1, T2, T3, T4>(
            IAdHocAction<T1, T2, T3, T4> action, 
            T1 obj1,
            T2 obj2,
            T3 obj3,
            T4 obj4);

        TOut ExecuteFunc<TIn, TOut>(
            IAdHocFunc<TIn, TOut> func, 
            TIn obj);

        TOut ExecuteFunc<TIn1, TIn2, TOut>(
            IAdHocFunc<TIn1, TIn2, TOut> func, 
            TIn1 obj1, 
            TIn2 obj2);

        TOut ExecuteFunc<TIn1, TIn2, TIn3, TOut>(
            IAdHocFunc<TIn1, TIn2, TIn3, TOut> func, 
            TIn1 obj1, 
            TIn2 obj2,
            TIn3 obj3);

    }
}
