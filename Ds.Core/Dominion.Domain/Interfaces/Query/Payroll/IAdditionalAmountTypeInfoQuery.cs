using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Payroll
{
    public interface IAdditionalAmountTypeInfoQuery : IQuery<AdditionalAmountTypeInfo, IAdditionalAmountTypeInfoQuery>
    {
        /// <summary>
        /// Queries by one or more specific amount types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IAdditionalAmountTypeInfoQuery ByTypes(params AdditionalAmountType[] types);
    }
}
