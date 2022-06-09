using System.Xml.Schema;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// A class for gathering the exception when you only want the basic exception functionality available.
    /// </summary>
    public class SchemaValidationErrMsg : MsgBase<SchemaValidationErrMsg>
    {
                 /// <summary>
        /// The exception data.
        /// </summary>
        public ValidationEventArgs Args { get; private set; }

        /// <summary>
        /// Constructor.
        /// By default this is a fatal error.
        /// </summary>
        /// <param name="args">Exception object.</param>
        public SchemaValidationErrMsg(ValidationEventArgs args)
            : base(MsgLevels.Fatal, MsgCodes.BasicException)
        {
            Args = args;
        }

        /// <summary>
        /// Method that builds the default message.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return Args.Message;
        }
    }
}