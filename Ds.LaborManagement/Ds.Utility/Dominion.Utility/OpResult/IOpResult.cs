using System;
using System.Collections.Generic;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.OpResult
{
    /// <summary>
    /// Expected functionality of a result object.
    /// </summary>
    public interface IOpResult
    {
        /// <summary>
        /// Flag to tell us if result was successful.
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Convenience method and better choice of words in some cases for reading the value of Success.
        /// In this case it will be the opposite of what ever success is.
        /// </summary>
        bool HasError { get; }

        /// <summary>
        /// Convenience method and better choice of words in some cases for reading the value of Success.
        /// In this case it will be the same value in success.
        /// </summary>
        bool HasNoError { get; }
            
        ///// <summary>
        ///// A collection of messages associated with this result.
        ///// </summary>
        // IList<IMsgSimple> Messages { get; set; }
        List<IMsgSimple> MsgObjects { get; set; }

        /// <summary>
        /// Registered actions to execute when <see cref="ExecutePostSuccessActions"/> is called.
        /// </summary>
        IEnumerable<Func<IOpResult>> PostSuccessActions { get; }

        /// <summary>
        /// Registered actions to execute when <see cref="ExecutePostSuccessActions"/> is called.
        /// </summary>
        IEnumerable<Func<IOpResult>> PostErrorActions { get; }

            /// <summary>
        /// Add a message to the list and get a chain return.
        /// </summary>
        /// <param name="msg">The messsage you want to add.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult AddMessage(IMsgSimple msg);

        /// <summary>
        /// Sets the success flag to true.
        /// Not sure if this is really needed but added it anyways.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult SetToSuccess();

        /// <summary>
        /// Sets the success flag to true.
        /// Not sure if this is really needed but added it anyways.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult EvaluateSuccess(bool criteria, IMsgSimple msg = null);

        /// <summary>
        /// Add a basic exception message to the result.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        IOpResult AddExceptionMessage(Exception ex);

        /// <summary>
        /// Set the success status basaed on the number of messages.
        /// If there are ANY messages, then success  false.
        /// </summary>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult SetSuccessBasedOnMessageCount();

        /// <summary>
        /// Set the success status based on the severity level of messages.
        /// If any messages are at least as severe as the minimum-to-failure level specified, then success false. 
        /// </summary>
        /// <param name="minToFailure">Minimum message severity required to fail the operation.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult SetSuccessBasedOnMessageLevel(MsgLevels minToFailure = MsgLevels.Error);

        /// <summary>
        /// Combine the messages and the success status.
        /// For combining the success the outcome will only change to false if the items your combining with is false.
        /// If the item you are combining is true and this is false this will not become true;
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult CombineSuccess(IOpResult otherResult);

        /// <summary>
        /// Take all the messages from the other result and add them to this object's list of messages.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult CombineMessages(IOpResult otherResult);

        /// <summary>
        /// Takes all registered post-result actions registered on another result and adds them to this object's actions.
        /// (see <see cref="AddPostSuccessAction"/> and <see cref="AddPostErrorAction"/>).
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns></returns>
        IOpResult CombinePostResultActions(IOpResult otherResult);

        /// <summary>
        /// Combine success and messages from other result.
        /// </summary>
        /// <param name="otherResult">Other IOpResult object.</param>
        /// <returns>Chained call; it returns this method's object instance.</returns>
        IOpResult CombineSuccessAndMessages(IOpResult otherResult);

        //IOpResult IfSuccess(Func<IOpResult> destFunc, Func<IOpResult> sourceFunc);
        IOpResult IfSuccess(OpResultFunc func, IOpResult otherResult);

        /// <summary>
        /// Adds an action to be executed when <see cref="ExecutePostSuccessActions"/> is called (after a 
        /// <see cref="Success"/>(ful) op-result).
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IOpResult AddPostSuccessAction(Func<IOpResult> action);

        /// <summary>
        /// Adds an action to be executed when <see cref="ExecutePostErrorActions"/> is called (after a 
        /// <see cref="Success"/>(ful) op-result).
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IOpResult AddPostErrorAction(Func<IOpResult> action);

        /// <summary>
        /// If op-result is <see cref="Success"/>(ful), will execute any action registered with 
        /// <see cref="AddPostSuccessAction"/> and returns the combined result.
        /// </summary>
        /// <returns></returns>
        IOpResult ExecutePostSuccessActions();

        /// <summary>
        /// If op-result is not <see cref="Success"/>(ful), will execute any action registered with 
        /// <see cref="AddPostErrorAction"/> and returns the combined result.
        /// </summary>
        /// <returns></returns>
        IOpResult ExecutePostErrorActions();

        /// <summary>
        /// If <see cref="HasError"/>, 
        /// concatinates all of the <see cref="MsgObjects"/>,
        /// and returns an <see cref="OpResultException"/> with the concatinated message.
        /// </summary>
        /// <returns><see cref="OpResultException"/> with the concatinated message.</returns>
        OpResultException GetExceptionForOpResultErrors();

        /// <summary>
        /// If <see cref="HasError"/>, 
        /// concatinates all of the <see cref="MsgObjects"/>,
        /// then throws an <see cref="OpResultException"/> with the concatinated message.
        /// </summary>
        /// <exception cref="OpResultException"></exception>
        [Obsolete("Avoid using this if possible. This shouldn't be used for typical control-flow.", false)]
        void ThrowExceptionForOpResultErrors();
    }
}