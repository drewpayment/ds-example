using System.Data.SqlClient;

namespace Dominion.Utility.Query
{
    public interface ISqlParameterBuilder
    {
        /// <summary>
        /// Builds the <see cref="SqlParameter"/>.
        /// </summary>
        /// <returns>SqlParameter.</returns>
        SqlParameter Build();
    }
}