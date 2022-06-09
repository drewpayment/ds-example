using System;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// A class for gathering the exception when you only want the basic exception functionality available.
    /// </summary>
    public class BasicExceptionMsg : MsgBase<BasicExceptionMsg>
    {
        /// <summary>
        /// The exception data.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Constructor.
        /// By default this is a fatal error.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        public BasicExceptionMsg(Exception ex)
            : base(MsgLevels.Fatal, MsgCodes.BasicException)
        {
            Exception = ex;
        }

        /// <summary>
        /// Method that builds the default message.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return Exception.ExceptionReport();
        }
    }
}