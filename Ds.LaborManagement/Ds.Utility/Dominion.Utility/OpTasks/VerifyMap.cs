using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class VerifyMap<TSource, TDest> : IVerifyMap<TSource, TDest> 
        where TSource : class 
        where TDest : class
    {
        #region Variables and Properties
        
        /// <summary>
        /// The object that contains the registered mappers and verifiers.
        /// </summary>
        private readonly IOpTasksFactory _factory;

        /// <summary>
        /// This should via the setup method.
        /// If null is passed in it will be set to a new instance
        /// </summary>
        public IOpResult OpResult { get; private set; }

        /// <summary>
        /// Represents the source object of the mapping scenario.
        /// Usually a DTO but not necessary.
        /// </summary>
        public TSource Source { get; set; }

        /// <summary>
        /// Represents the destination object of the mapping scenario.
        /// Usually an entity but not necessary.
        /// </summary>
        public TDest Dest { get; set; }

        #endregion

        #region Constructor

        public VerifyMap(IOpTasksFactory factory, TSource source, IOpResult opResult)
        {
            _factory = factory;
            OpResult = opResult ?? new OpResult.OpResult();
            Source = source;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Map using the internal factory to resolve the mapper.
        /// </summary>
        /// <returns></returns>
        public IVerifyMap<TSource, TDest> Map()
        {
            if(OpResult.Success)
            {
                var mapper = _factory.GetMapper<TSource, TDest>();
                
                OpResult.TryCatch(() =>
                {
                    Dest = mapper.Map(Source);
                });
            }

            return this;
        }


        public IVerifyMap<TSource, TDest> Map(Func<TSource, TDest> mapFunc, bool requireSuccess)
        {
            if(!requireSuccess || OpResult.Success)
            {
                OpResult.TryCatch(() =>
                {
                    Dest = mapFunc(Source);
                });
            }

            return this;
        }

        /// <summary>
        /// Verify using the internal factory to resolve the verifier.
        /// </summary>
        /// <returns></returns>
        public IVerifyMap<TSource, TDest> ValidateDest()
        {
            if(OpResult.Success)
            {
                var verifier = _factory.GetVerifier<TDest>();

                OpResult.TryCatch(() =>
                {
                    OpResult.CombineSuccessAndMessages(verifier.Verify(Dest));
                });
            }

            return this;
        }

        #endregion
    }
}
