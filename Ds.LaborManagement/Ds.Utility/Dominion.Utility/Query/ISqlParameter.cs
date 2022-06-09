using System.Data;

namespace Dominion.Utility.Query
{
    public interface ISqlParameter<TVal>
    {
        /// <summary>
        /// The name of the parameter as it is on the sproc.
        /// Include the '@' in front of the name.
        /// </summary>
        string ParamName { get; }

        /// <summary>
        /// The SQL type this parameter represents.
        /// </summary>
        SqlDbType SqlType { get; }

        /// <summary>
        /// The .NET type this parameter represents.
        /// </summary>
        TVal Value { get; set; }
    }
}