using System;
using System.Collections.Generic;
using System.Linq;

using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Validation;

namespace Dominion.Utility.OpTasks
{
    /// <summary>
    /// Injected into an API session to aide in building object for carring out common tasks.
    /// ie: Map and Validate objects: <see cref="VerifyMap{TSource, TDest}"/>
    /// </summary>
    public class OpTasksFactory : IOpTasksFactory
    {
        #region Variables and Properties

        private readonly Dictionary<Type, List<object>> _ruleDictionary;
        private readonly Dictionary<Type, object> _verifyDictionary;

        #endregion

        #region Constructors and Initializers

        public OpTasksFactory()
        {
            _ruleDictionary = new Dictionary<Type, List<object>>();
            _verifyDictionary = new Dictionary<Type, object>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of IVeryifyMap.
        /// </summary>
        /// <typeparam name="TSource">The source type.</typeparam>
        /// <typeparam name="TDest">The destination type.</typeparam>
        /// <returns></returns>
        IVerifyMap<TSource, TDest> IOpTasksFactory.BuildVerifyMap<TSource, TDest>(TSource obj, IOpResult result)
        {
            var verifyMapper = new VerifyMap<TSource, TDest>(this, obj, result);
            return verifyMapper;
        }

        /// <summary>
        /// Get a mapper object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="TSource">The source object.</typeparam>
        /// <typeparam name="TDest">The destination object.</typeparam>
        /// <returns></returns>
        IMapper<TSource, TDest> IOpTasksFactory.GetMapper<TSource, TDest>()
        {
            var sourceType = typeof(TSource);
            var listOfMappers = default(List<object>);

            if(_ruleDictionary.TryGetValue(sourceType, out listOfMappers))
            {
                var mapper = listOfMappers.FirstOrDefault(x => x is IMapper<TSource, TDest>);
                return mapper as IMapper<TSource, TDest>;
            }

            return null;
        }

        /// <summary>
        /// Get a verifier object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="T">The object to validate.</typeparam>
        /// <returns></returns>
        IVerify<T> IOpTasksFactory.GetVerifier<T>()
        {
            var verifier = default(object);
            
            if(_verifyDictionary.TryGetValue(typeof(T), out verifier))
                return verifier as IVerify<T>;

            return null;
        }

        /// <summary>
        /// Get a verifier (object validation ie. fluent validation) object from the registry (dictionary).
        /// </summary>
        /// <typeparam name="T">Type to validate.</typeparam>
        /// <typeparam name="TVerify">Type of verifier to get.</typeparam>
        /// <returns></returns>
        TVerify IOpTasksFactory.GetVerifier<T, TVerify>()
        {
            object verifier;
            
            if(_verifyDictionary.TryGetValue(typeof(T), out verifier))
                return (TVerify)verifier;

            return default(TVerify);
        }

        /// <summary>
        /// Register a mapper object.
        /// </summary>
        /// <typeparam name="TSource">The source object.</typeparam>
        /// <typeparam name="TDest">The destination object.</typeparam>
        /// <param name="mapper"></param>
        IOpTasksFactory IOpTasksFactory.RegisterMapper<TSource, TDest>(IMapper<TSource, TDest> mapper)
        {
            var sourceType = typeof(TSource);
            var value = default(List<object>);

            if(_ruleDictionary.TryGetValue(sourceType, out value))
                value.Add(mapper);
            else
                _ruleDictionary.Add(sourceType, new List<object>(){ mapper });

            return this;
        }

        /// <summary>
        /// Register a validator.
        /// </summary>
        /// <typeparam name="T">The object being validated.</typeparam>
        /// <param name="verifier">The validator object.</param>
        IOpTasksFactory IOpTasksFactory.RegisterValidator<T>(IVerify<T> verifier)
        {
            var objType = typeof(T);

            if(!_verifyDictionary.ContainsKey(objType))
                _verifyDictionary.Add(objType, verifier);

            return this;
        }


        void IOpTasksFactory.ExecuteAction<T>(
            IAdHocAction<T> action, 
            T obj)
        {
            action.Execute(obj);
        }

        void IOpTasksFactory.ExecuteAction<T1, T2>(IAdHocAction<T1, T2> action, T1 obj1, T2 obj2)
        {
            action.Execute(obj1, obj2);
        }

        void IOpTasksFactory.ExecuteAction<T1, T2, T3>(
            IAdHocAction<T1, T2, T3> action, 
            T1 obj1, 
            T2 obj2, 
            T3 obj3)
        {
            action.Execute(obj1, obj2, obj3);
        }

        void IOpTasksFactory.ExecuteAction<T1, T2, T3, T4>(
            IAdHocAction<T1, T2, T3, T4> action, 
            T1 obj1, 
            T2 obj2, 
            T3 obj3, 
            T4 obj4)
        {
            action.Execute(obj1, obj2, obj3, obj4);
        }

        TOut IOpTasksFactory.ExecuteFunc<TIn, TOut>(
            IAdHocFunc<TIn, TOut> func, 
            TIn obj)
        {
            return func.Execute(obj);
        }

        TOut IOpTasksFactory.ExecuteFunc<TIn1, TIn2, TOut>(
            IAdHocFunc<TIn1, TIn2, TOut> func, 
            TIn1 obj1, 
            TIn2 obj2)
        {
            return func.Execute(obj1, obj2);
        }

        TOut IOpTasksFactory.ExecuteFunc<T1, T2, T3, TOut>(
            IAdHocFunc<T1, T2, T3, TOut> func, 
            T1 obj1, 
            T2 obj2, 
            T3 obj3)
        {
            return func.Execute(obj1, obj2, obj3);
        }

        #endregion

    }
}
