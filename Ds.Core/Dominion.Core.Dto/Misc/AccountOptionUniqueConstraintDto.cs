using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Misc
{
    /// <summary>
    /// Property names correspond to the actual column names in dbo.ClientOption
    /// </summary>
    public interface IAccountOptionUniqueConstraintDto
    {
        int ClientId { get; }
        AccountOption ClientOptionControlId { get; }
    }

    /// <summary>
    /// Property names correspond to the actual column names in dbo.ClientOption
    /// </summary>
    public interface IAccountOptionUniqueConstraintWithIdAndValueDto : IAccountOptionUniqueConstraintDto
    {
        int ClientAccountOptionId { get; }
        string Value { get; set; }
    }

    /// <inheritdoc cref="IAccountOptionUniqueConstraintDto"/>
    public class AccountOptionUniqueConstraintDto : IAccountOptionUniqueConstraintDto
    {
        public int ClientId { get; set; }
        public AccountOption ClientOptionControlId { get; set; }
    }

    /// <inheritdoc cref="IAccountOptionUniqueConstraintWithIdAndValueDto"/>
    public class AccountOptionUniqueConstraintWithIdAndValueDto : IAccountOptionUniqueConstraintWithIdAndValueDto
    {
        public int ClientAccountOptionId { get; set; }
        public int ClientId { get; set; }
        public AccountOption ClientOptionControlId { get; set; }
        public string Value { get; set; }
    }
}
