using System;

using Dominion.Utility.Msg.Identifiers;

namespace Dominion.Utility.Msg.Specific
{
    /// <summary>
    /// Basic message used to indicate an object or collection did not contain data as expected.
    /// </summary>
    /// <typeparam name="TData">Type of data which was not found.</typeparam>
    public class DataNotFoundMsg<TData> : MsgBase<DataNotFoundMsg<TData>>
    {
        /// <summary>
        /// The type of data which was not found.
        /// </summary>
        public Type TypeNotFound
        {
            get
            {
                return typeof(TData);
            }
        }

        #region Constructor

        /// <summary>
        /// Instantiates a new <see cref="DataNotFoundMsg{TData}"/>.
        /// </summary>
        /// <param name="level"></param>
        public DataNotFoundMsg(MsgLevels level)
            : base(level, MsgCodes.DataNotFound)
        {
        }

        #endregion

        /// <summary>
        /// Returns a message indicating what type of data was not found.
        /// </summary>
        /// <returns></returns>
        protected override string BuildMsg()
        {
            return "The expected " + TypeNotFound.FullName + " was not found.";
        }
    }
}
