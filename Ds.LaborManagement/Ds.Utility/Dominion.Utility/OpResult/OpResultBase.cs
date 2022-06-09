using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Utility.Logging;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;
using Dominion.Utility.Msg.Specific;
using Serilog;
using Serilog.Events;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Odd are you won't need to derive from this base class.
    /// It is however used as a base class for those classes that you should.
    /// Chris Angel has nothing on this.
    /// </summary>
    /// <typeparam name="TChainReturn"></typeparam>
    public abstract class OpResultBase<TChainReturn> : IOpResult
        where TChainReturn : class, IOpResult
    {
        #region Variables and Properties

        protected IDsLogger Logger { get; set; }

        private readonly List<Func<IOpResult>> _successActions;

        private readonly List<Func<IOpResult>> _errorActions;

        /// <summary>
        /// The chained object.
        /// </summary>
        private readonly TChainReturn _chainReturn;

        /// <summary>
        /// Flag to tell us if result was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Convenience method and better choice of words in some cases for reading the value of Success.
        /// In this case it will be the opposite of what ever success is.
        /// </summary>
        public bool HasError
        {
            get { return !Success; }
        }

        /// <summary>
        /// Convenience method and better choice of words in some cases for reading the value of Success.
        /// In this case it will be the same value in success.
        /// </summary>
        public bool HasNoError
        {
            get { return Success; }
        }

        public List<IMsgSimple> MsgObjects { get; set; }

        /// <summary>
        /// Registered actions to execute when <see cref="IOpResult.ExecutePostSuccessActions"/> is called.
        /// </summary>
        public IEnumerable<Func<IOpResult>> PostSuccessActions { get { return _successActions; } }

        /// <summary>
        /// Registered actions to execute when <see cref="IOpResult.ExecutePostSuccessActions"/> is called.
        /// </summary>
        public IEnumerable<Func<IOpResult>> PostErrorActions { get { return _errorActions; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor yo.
        /// </summary>
        protected OpResultBase()
        {
            Success        = true;
            MsgObjects     = new List<IMsgSimple>();
            _successActions = new List<Func<IOpResult>>();
            _errorActions   = new List<Func<IOpResult>>();
            _chainReturn   = ChainReturn();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a message to the list and get a chain return.
        /// </summary>
        /// <param name="msg">The messsage you want to add.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult AddMessage(IMsgSimple msg)
        {
            LogMessage(msg);
            MsgObjects.Add(msg);
            return _chainReturn;
        }

        /// <summary>
        /// Add a list of messages to the list and get a chain return.
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public IOpResult AddMessages(IEnumerable<IMsgSimple> messages)
        {
            MsgObjects.AddRange(messages);
            return _chainReturn;
        }

        /// <summary>
        /// Sets the success flag to true.
        /// Not sure if this is really needed but added it anyways.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult SetToSuccess()
        {
            Success = true;
            return _chainReturn;
        }

        /// <summary>
        /// Sets the success flag to true.
        /// Not sure if this is really needed but added it anyways.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult EvaluateSuccess(bool criteria, IMsgSimple msg = null)
        {
            Success = criteria;

            if(!Success && msg != null)
                AddMessage(msg);

            return _chainReturn;
        }

        /// <summary>
        /// Add a basic exception message to the result.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public IOpResult AddExceptionMessage(Exception ex)
        {
            AddMessage(new BasicExceptionMsg(ex));
            return _chainReturn;
        }

        /// <summary>
        /// Set the success status basaed on the number of messages.
        /// If there are ANY messages, then success == false.
        /// This will only set the success to FALSE, never true.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult SetSuccessBasedOnMessageCount()
        {
            Success = Success ? MsgObjects.Count == 0 : Success;
            return _chainReturn;
        }

        /// <summary>
        /// Set the success status based on the severity level of messages.
        /// If any messages are at least as severe as the minimum-to-failure level specified, then success false. 
        /// </summary>
        /// <param name="minToFailure">Minimum message severity required to fail the operation.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult SetSuccessBasedOnMessageLevel(MsgLevels minToFailure = MsgLevels.Error)
        {
            Success = Success ? MsgObjects.All(msg => msg.Level < minToFailure) : Success;
            return this;
        }

        /// <summary>
        /// Combine the messages and the success status.
        /// For combining the success the outcome will only change to false if the items your combining with is false.
        /// If the item you are combining is true and this is false this will not become true;
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult CombineSuccess(IOpResult otherResult)
        {
            if (otherResult != null && otherResult.HasError)
                Success = false;

            return _chainReturn;
        }

        /// <summary>
        /// Take all the messages from the other result and add them to this object's list of messages.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult CombineMessages(IOpResult otherResult)
        {
            if (otherResult != null)
            {
                foreach (var msg in otherResult.MsgObjects)
                {
                    LogMessage(msg);
                }
                MsgObjects.AddRange(otherResult.MsgObjects);
            }

            return _chainReturn;
        }

        /// <summary>
        /// Take all <see cref="IOpResult.PostSuccessActions"/> and <see cref="IOpResult.PostErrorActions"/> and add them to this object's action
        /// lists.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns></returns>
        public IOpResult CombinePostResultActions(IOpResult otherResult)
        {
            if (otherResult != null && otherResult.PostSuccessActions != null)
                _successActions.AddRange(otherResult.PostSuccessActions);

            if (otherResult != null && otherResult.PostErrorActions != null)
               _errorActions.AddRange(otherResult.PostErrorActions);

            return this;
        }

        /// <summary>
        /// Combine success and messages from other result.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        public IOpResult CombineSuccessAndMessages(IOpResult otherResult)
        {
            return
                CombineMessages(otherResult)
                .CombineSuccess(otherResult)
                .CombinePostResultActions(otherResult);
        }

        public IOpResult IfSuccess(OpResultFunc func, IOpResult otherResult)
        {
            if(Success)
            {
                switch(func)
                {
                    case OpResultFunc.CombineSuccessAndMessages:
                        return CombineSuccessAndMessages(otherResult);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("func");
                }                
            }

            return _chainReturn;
        }

        public IOpResult AddPostSuccessAction(Func<IOpResult> action)
        {
            _successActions.Add(action);
            return this;
        }

        public IOpResult AddPostErrorAction(Func<IOpResult> action)
        {
            _errorActions.Add(action);
            return this;
        }

        /// <summary>
        /// Upon <see cref="IOpResult.Success"/>, will execute all <see cref="IOpResult.PostSuccessActions"/> and return the combined result.
        /// </summary>
        /// <returns></returns>
        public IOpResult ExecutePostSuccessActions()
        {
            var result = new OpResult();

            if(Success)
            {
                foreach (var action in PostSuccessActions)
                {
                    action().MergeInto(result);
                }
            }

            return result;
        }

        /// <summary>
        /// Upon <see cref="IOpResult.HasError"/>, will execute all <see cref="IOpResult.PostErrorActions"/> and return the combined result.
        /// </summary>
        /// <returns></returns>
        public IOpResult ExecutePostErrorActions()
        {
            var result = new OpResult();

            if (HasError)
            {
                foreach (var action in PostErrorActions)
                {
                    action().MergeInto(result);
                }
            }

            return result;
        }

        /// <inheritdoc/>
        public OpResultException GetExceptionForOpResultErrors()
        {
            if (this.HasError)
            {
                string message = string.Join(
                    Environment.NewLine,
                    this.MsgObjects.Select(x => x.Msg)
                );
                return new OpResultException(message);
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="OpResultException"></exception>
        [Obsolete("Avoid using this if possible. This shouldn't be used for typical control-flow.", false)]
        public void ThrowExceptionForOpResultErrors()
        {
            var ex = this.GetExceptionForOpResultErrors();

            if (this.HasError && !(ex is null))
            {
                throw ex;
            }
        }

        public IOpResult UpdateLogger(IDsLogger logger)
        {
            Logger = logger;
            return this;
        }

        private void LogMessage(IMsgSimple msg)
        {
            GetLogger()?.LogMessage(msg);

            //Log.Logger.Write(GetLogEventLevelFromMsgLevels(msg.Level), "{msg}", msg);
        }

        private IDsLogger GetLogger() => Logger ?? OpResultConstants.DefaultLogger;

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Used to return the constructed type not this (the base).
        /// </summary>
        /// <returns></returns>
        protected abstract TChainReturn ChainReturn();

        #endregion
    }

    public enum OpResultFunc
    {
        CombineSuccessAndMessages = 1,
    }
}