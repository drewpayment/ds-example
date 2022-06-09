using CsvHelper;
using Dominion.Utility.Msg;
using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Csv
{
    /// <summary>
    /// Message wrapper for a <see cref="CsvHelperException" />.
    /// </summary>
    public class CsvHelperExceptionMsg : MsgBase<CsvHelperExceptionMsg>
    {
        /// <summary>
        /// <see cref="CsvHelperException" /> contianing details of the error.
        /// </summary>
        public CsvHelperException Exception { get; private set; }

        /// <summary>
        /// Instantiates a new <see cref="CsvHelperExceptionMsg"/>.
        /// </summary>
        /// <param name="exception"><see cref="CsvHelperException"/> to construct the message around.</param>
        public CsvHelperExceptionMsg(CsvHelperException exception)
            : base(MsgLevels.Fatal, 0)
        {
            Exception = exception;
        }

        /// <summary>
        /// Build a message containing basic details of the CSV exception that was thrown.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return "CSV Error: " + Exception.Message;
        }
    }
}