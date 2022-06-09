using Dominion.Domain.Entities.Onboarding;
using Dominion.Taxes.Dto.TaxTypes;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingW4Query : IQuery<EmployeeOnboardingW4, IOnboardingW4Query>
    {
        /// <summary>
        /// Filters W4s by the specified employee.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IOnboardingW4Query ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters W4s by the specified tax category.
        /// </summary>
        /// <param name="taxCategory">Type of tax to get W4 for</param>
        /// <returns></returns>
        IOnboardingW4Query ByTaxCategory(TaxCategory taxCategory);
    }
}
