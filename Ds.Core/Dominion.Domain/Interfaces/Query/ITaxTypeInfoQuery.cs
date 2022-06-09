using Dominion.Domain.Entities.Tax;
using Dominion.Taxes.Dto.TaxTypes;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Represents a query that can be executed on a set of client earnings.
    /// </summary>
    public interface ITaxTypeInfoQuery : IQuery<TaxTypeInfo, ITaxTypeInfoQuery>
    {
        /// <summary>
        /// Filters tax types by the given <see cref="TaxCategory"/>.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        ITaxTypeInfoQuery ByTaxCategory(TaxCategory category);

        /// <summary>
        /// Filters tax types which employees (EE) must pay taxes on.
        /// </summary>
        /// <param name="isEmployeeTax">True if employee must pay tax.</param>
        /// <returns></returns>
        ITaxTypeInfoQuery ByIsEmployeeTax(bool isEmployeeTax);

        /// <summary>
        /// Filters tax types which employers (ER) must pay taxes on.
        /// </summary>
        /// <param name="isEmployerTax">True if employers must pay tax.</param>
        /// <returns></returns>
        ITaxTypeInfoQuery ByIsEmployerTax(bool isEmployerTax);

        /// <summary>
        /// Filters by tax types that are payable by employees only (EE).
        /// </summary>
        /// <returns></returns>
        ITaxTypeInfoQuery ByIsEmployeeOnly();

        /// <summary>
        /// Filters by tax types that are payable by employers only (ER).
        /// </summary>
        /// <returns></returns>
        ITaxTypeInfoQuery ByIsEmployerOnly();
    }
}
